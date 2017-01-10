using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Project = Microsoft.CodeAnalysis.Project;
using Solution = Microsoft.CodeAnalysis.Solution;
using TFSWorkspace = Microsoft.TeamFoundation.VersionControl.Client.Workspace;

namespace InterfaceGenerator
{

    public partial class FrmInterfaceGenerator : Form
    {
        private const string InterfacePrefix = "I";
        private int sortColumn = -1;
        private TFSWorkspace tfsWorkspace;
        private string slnPath = string.Empty;
        private Solution solution;
        private readonly ListView lstBackup = new ListView();
        //private Dictionary<string, IEnumerable<SyntaxTree>> syntaxTrees = new Dictionary<string, IEnumerable<SyntaxTree>>();
        readonly List<ProjectInfo> projectInfos = new List<ProjectInfo>();
        private MSBuildWorkspace buildWorkspace;
        public FrmInterfaceGenerator()
        {
            InitializeComponent();
            //btnLog.Enabled = File.Exists("buildlog.txt");
        }

        private void btnSelectSlnFolder_Click(object sender, EventArgs e)
        {
            var result = slnFileOpen.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSelectSlnFolder.Text = slnFileOpen.FileName;
                if (slnPath != txtSelectSlnFolder.Text)
                {
                    Clear();
                }
                slnPath = txtSelectSlnFolder.Text;
            }
        }

        private void btnGetFiles_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                UseWait(s =>
                {
                    btnGetFiles.Enabled = false;
                    btnCompile.Enabled = false;
                    BindSourceControl();
                    GetLatest();
                    btnCompile.Enabled = true;
                    btnGetFiles.Enabled = true;
                }, "Getting latest from source control...");
            }
        }


        public void Compile()
        {
            var addedAssemblies = new List<string>();
            buildWorkspace = MSBuildWorkspace.Create();
            solution = buildWorkspace.OpenSolutionAsync(slnPath).Result;
            var projects = solution.Projects.ToList();

            progressBar1.Show();
            var documentCount = projects.SelectMany(x => x.Documents).Count();
            progressBar1.Maximum = documentCount * 100;
            progressBar1.Step = documentCount * 10;

            lstPreExtractFiles.BeginUpdate();

            var addedNamespaces = new List<string>();
            foreach (var project in projects)
            {
                lblInfo.Text = $@"Compiling project {project.Name}";
                var compilation = project.GetCompilationAsync().Result;
                if (!addedAssemblies.Contains(compilation.AssemblyName))
                {
                    cmbAssembly.Items.Add(compilation.AssemblyName);
                    addedAssemblies.Add(compilation.AssemblyName);
                }
                foreach (var document in project.Documents.Where(x => x.Name != "AssemblyInfo.cs"))
                {
                    projectInfos.Add(new ProjectInfo
                    {
                        FilePath = document.FilePath,
                        Project = document.Project,
                        SyntaxTrees = compilation.SyntaxTrees
                    });
                }

                foreach (var tree in compilation.SyntaxTrees)
                {
                    var nodes = tree.GetCompilationUnitRoot().DescendantNodes().ToList();
                    var @classes = nodes.OfType<ClassDeclarationSyntax>();
                    var nameSpace = nodes.OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
                    foreach (var @class in classes)
                    {
                        if (nameSpace == null) continue;
                        var item = new ListViewItem { Text = compilation.AssemblyName };
                        var nsName = compilation.GetSemanticModel(tree).GetDeclaredSymbol(@class).ContainingNamespace.ToDisplayString();
                        if (!addedNamespaces.Exists(x => x == nsName))
                        {
                            cmbNamespace.Items.Add(nsName);
                            addedNamespaces.Add(nsName);
                        }
                        item.SubItems.Add(new ListViewItem.ListViewSubItem
                        {
                            Text = compilation.GetSemanticModel(tree).GetDeclaredSymbol(@class).ContainingNamespace.ToDisplayString()
                        });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem
                        {
                            Text = compilation.GetSemanticModel(tree).GetDeclaredSymbol(@class).Name
                        });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem
                        {
                            Text = string.Join(",", compilation.GetSemanticModel(tree).GetDeclaredSymbol(@class).Interfaces.Select(x => x.Name))
                        });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem
                        {
                            Text = @class.SyntaxTree.FilePath
                        });
                        lstPreExtractFiles.Items.Add(item);
                    }
                }
                progressBar1.PerformStep();
            }

            lstPreExtractFiles.EndUpdate();
            panel1.Hide();
            lstBackup.Items.AddRange((from ListViewItem item in lstPreExtractFiles.Items
                                      select (ListViewItem)item.Clone()).ToArray());
        }

        private void Clear()
        {

            cmbAssembly.Items.Clear();
            cmbNamespace.Items.Clear();
            cmbAssembly.Text = string.Empty;
            cmbNamespace.Text = string.Empty;
            chkSubFolder.Checked = false;

            foreach (ListViewItem item in lstPreExtractFiles.Items)
            {
                lstPreExtractFiles.Items.Remove(item);
                lstBackup.Items.Remove(item);
            }
            progressBar1.Value = 0;


        }

        private void ClearFilter()
        {
            UseWait(s =>
            {
                cmbAssembly.Text = string.Empty;
                cmbNamespace.Text = string.Empty;
                foreach (var assembly in cmbAssembly.CheckBoxItems)
                {
                    ReloadAssembly(assembly.Text);
                }
                foreach (var nameSpace in cmbNamespace.CheckBoxItems)
                {
                    ReloadNamespace(nameSpace.Text);
                }
            }, "Please wait while we clear the filter");
        }

        private void lstPreExtractFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                lstPreExtractFiles.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                lstPreExtractFiles.Sorting = lstPreExtractFiles.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            lstPreExtractFiles.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            lstPreExtractFiles.ListViewItemSorter = new ListViewItemComparer(e.Column, lstPreExtractFiles.Sorting);

        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                UseWait(s =>
                {
                    btnExtract.Enabled = false;
                    chkSubFolder.Enabled = false;
                    Extract();
                    btnExtract.Enabled = true;
                    chkSubFolder.Enabled = true;
                }, "Please wait while we extract the interfaces...");
            }
        }

        private void Extract()
        {
            var newlyAddedFiles = new List<ProjectInfo>();

            foreach (ListViewItem item in lstPreExtractFiles.CheckedItems)
            {
                var className = item.SubItems[2].Text;
                var projectInfo = projectInfos.FirstOrDefault(x => x.FilePath.ToLower() == item.SubItems[4].Text.ToLower());
                if (projectInfo == null) continue;

                var folderPath = Path.GetDirectoryName(projectInfo.FilePath);
                var subFolder = string.Empty;
                subFolder = SetSubFolder(folderPath, subFolder);

                var syntaxTree = projectInfo.SyntaxTrees;
                var sbInterface = new StringBuilder();
                var tree = syntaxTree.FirstOrDefault(x => x.FilePath == projectInfo.FilePath);
                if (tree == null) continue;
                {
                    var nodes = tree.GetCompilationUnitRoot().DescendantNodesAndSelf().ToList();
                    var @classes = nodes.OfType<ClassDeclarationSyntax>();
                    var @namespace = nodes.OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
                    var fullyQualifiedInterfaceName = string.Empty;
                    foreach (var @class in classes)
                    {
                        if (className != @class.Identifier.ValueText) continue;
                        if (@class.Modifiers.FirstOrDefault().ValueText != "public") continue;

                        var interfaceName = InterfacePrefix + className;
                        var @properties = @class.Members.OfType<PropertyDeclarationSyntax>().ToList();
                        var @methods = @class.Members.OfType<MethodDeclarationSyntax>().ToList();
                        
                        #region creating Interface file
                        tree.GetCompilationUnitRoot().Usings.ToList().ForEach(x =>
                        {
                            sbInterface.AppendLine(x.ToString());
                        });
                        sbInterface.AppendLine();

                        if (@namespace != null)
                        {
                            fullyQualifiedInterfaceName = $"{@namespace.Name}.Interface";
                            sbInterface.AppendLine(chkSubFolder.Checked
                                ? $"namespace {fullyQualifiedInterfaceName}"
                                : $"namespace {@namespace.Name}");
                            sbInterface.AppendLine("{");
                        }

                        sbInterface.AppendLine($"\tpublic interface {interfaceName}");
                        sbInterface.AppendLine("\t{");


                        foreach (var property in properties)
                        {
                            if (property.Modifiers.FirstOrDefault().ValueText != "public") continue;
                            var prop = property.Type + " " + property.Identifier.Text + " { ";
                            foreach (var accessor in property.AccessorList.Accessors)
                            {
                                prop += accessor.Keyword.Text + ";";
                            }
                            prop += " } ";
                            sbInterface.AppendLine("\t\t" + prop);
                        }
                        foreach (var method in @methods)
                        {
                            if (method.Modifiers.FirstOrDefault().ValueText == "public")
                            {
                                sbInterface.AppendLine("\t\t" + method.ReturnType + " " +
                                                       method.Identifier.ToFullString() +
                                                       " (" +
                                                       string.Join(",", method.ParameterList.Parameters) + ");");
                            }
                        }
                        sbInterface.AppendLine("\t}");
                        if(@namespace != null)
                            sbInterface.AppendLine("}");
                        #endregion

                        if (
                            @class.BaseList.Contains(
                                SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(interfaceName))))
                            continue;
                         
                        var newClass =  @class.AddBaseListTypes(
                                SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(interfaceName)));

                        var compilationUnitSyntax =
                            tree.GetCompilationUnitRoot().ReplaceNode(@class, newClass).NormalizeWhitespace();

                        compilationUnitSyntax =
                            compilationUnitSyntax.AddUsings(
                                    SyntaxFactory.UsingDirective(
                                        SyntaxFactory.IdentifierName(fullyQualifiedInterfaceName)))
                                .NormalizeWhitespace(elasticTrivia: true);

                        var fullPath = (chkSubFolder.Checked ? subFolder : folderPath) + "\\" + interfaceName +
                                       ".cs";
                        newlyAddedFiles.Add(new ProjectInfo
                        {
                            FilePath = fullPath,
                            ClassFilePath = item.SubItems[4].Text,
                            Project = projectInfo.Project,
                            CompilationUnit = compilationUnitSyntax
                        });
                        if (!File.Exists(fullPath))
                        {
                            File.WriteAllText(fullPath, sbInterface.ToString());
                        }
                        item.SubItems[3].Text = interfaceName;
                        item.SubItems[4].Text = fullPath;

                        //projectInfo.Project = AddFileToProject(solution.GetProject(projectInfo.Project.Id), fullPath, item.SubItems[4].Text, compilationUnitSyntax);
                    }
                }
            }
            if (!newlyAddedFiles.Any()) return;
            Project project = null;
            foreach (var newlyAddedFile in newlyAddedFiles.OrderBy(a => a.Project.Name))
            {
                if (project == null || project.Name != newlyAddedFile.Project.Name)
                {
                    project = solution.GetProject(newlyAddedFile.Project.Id);
                }

                project = AddFileToProject(project, newlyAddedFile.FilePath, newlyAddedFile.ClassFilePath, newlyAddedFile.CompilationUnit);
            }
            buildWorkspace.TryApplyChanges(solution);
        }

        private string SetSubFolder(string folderPath, string subFolder)
        {
            if (!chkSubFolder.Checked) return subFolder;
            tfsWorkspace?.PendEdit(folderPath);
            subFolder = folderPath + "\\Interface";
            if (Directory.Exists(subFolder)) return subFolder;
            var di = Directory.CreateDirectory(subFolder);
            var dSecurity = di.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(
                System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));
            di.SetAccessControl(dSecurity);
            di.Attributes = FileAttributes.Normal;
            if (chkSubFolder.Checked)
            {
                tfsWorkspace?.PendAdd(subFolder, true);
            }
            return subFolder;
        }
        private Project AddFileToProject(Project project, string fullPath, string classFilePath, SyntaxNode root)
        {
            if (!buildWorkspace.CanApplyChange(ApplyChangesKind.AddDocument)) return project;

            var newDoc = project.AddDocument(Path.GetFileName(fullPath), File.ReadAllText(fullPath),
                new[] { Path.GetDirectoryName(fullPath) }, fullPath);
            project = newDoc.Project;

            if (!string.IsNullOrWhiteSpace(classFilePath))
            {
                var classDoc = project.Documents.FirstOrDefault(x => x.FilePath == classFilePath);
                if (classDoc != null)
                {
                    classDoc = classDoc.WithSyntaxRoot(root);
                    project = classDoc.Project;
                }
            }
            solution = project.Solution;
            tfsWorkspace?.PendEdit(project.FilePath);
            tfsWorkspace?.PendAdd(Path.GetDirectoryName(fullPath), true);
            return project;
        }

        #region TFS Source Control
        private void BindSourceControl()
        {
            // Get all registered project collections
            var projectCollections = new List<RegisteredProjectCollection>(RegisteredTfsConnections.GetProjectCollections());

            foreach (var registeredProjectCollection in projectCollections)
            {
                var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(registeredProjectCollection);
                tfs.EnsureAuthenticated();


                var createdWorkspace = false;
                try
                {
                    var versionControl = tfs.GetService<VersionControlServer>();

                    var teamProjects = new List<TeamProject>(versionControl.GetAllTeamProjects(false));

                    if (teamProjects.Count < 1)
                        continue;
                    string workspaceName;
                    string serverFolder;
                    WorkingFolder workingFolder;

                    var workspaceSet = SetWorkspace(versionControl, teamProjects, out workspaceName, ref createdWorkspace, out serverFolder,
                         out workingFolder);
                    if (!workspaceSet)
                    {
                        DialogResult = DialogResult.Abort;
                        Close();
                    }

                    //// Determine whether the user has check-in permissions.
                    //if (workspace != null && !workspace.HasCheckInPermission)
                    //{
                    //	MessageBox.Show(this, $@"{workspace.VersionControlServer.AuthorizedUser} does not have check-in permission for workspace {workspace.DisplayName}", @"Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                    //  Create a list of pending changes.
                    //GetPendings(serverFolder, versionControl, workspaceName);

                    // Check in the items that you added.
                    //var changesetForAdd = workspace.CheckIn(pendingAdds.ToArray(), "Initial revision");
                    //Console.WriteLine("Checked in changeset {0}", changesetForAdd);


                }
                finally
                {
                    if ((tfsWorkspace != null) && createdWorkspace)
                    {
                        tfsWorkspace.Delete();
                    }
                }
                break;
            }
        }

        private void GetLatest()
        {
            var fi = new FileInfo(slnPath);
            var directory = fi.Directory?.Parent;
            if (directory == null) return;
            var itemSpecs = new ItemSpec[1];
            itemSpecs[0] = new ItemSpec(directory.FullName, RecursionType.Full);
            tfsWorkspace.Get(new[] { new GetRequest(new ItemSpec(directory.FullName, RecursionType.Full), VersionSpec.Latest) },
                GetOptions.GetAll);
        }

        private bool SetWorkspace(VersionControlServer versionControl, List<TeamProject> teamProjects,
            out string workspaceName,
            ref bool createdWorkspace, out string serverFolder, out WorkingFolder workingFolder)
        {
            workspaceName = Environment.MachineName;
            workingFolder = null;
            serverFolder = null;
            try
            {
                if (string.IsNullOrWhiteSpace(slnPath))
                {
                    MessageBox.Show(this,
                    @"Solution/Project is not selected.  Please select one and open this tool again",
                    @"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(Path.GetDirectoryName(slnPath));
                if (workspaceInfo != null)
                {
                    tfsWorkspace = versionControl.GetWorkspace(workspaceInfo);
                }
                else
                {
                    MessageBox.Show(this, @"Could not find the workspace.", @"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            catch (WorkspaceNotFoundException)
            {
                tfsWorkspace = versionControl.CreateWorkspace(workspaceName, versionControl.AuthorizedUser);
                createdWorkspace = true;
            }
            try
            {
                serverFolder = tfsWorkspace.GetServerItemForLocalItem(slnPath);
                var localFolder = tfsWorkspace.GetLocalItemForServerItem(serverFolder);
                workingFolder = new WorkingFolder(serverFolder, localFolder);
                serverFolder = tfsWorkspace.GetServerItemForLocalItem(slnPath);

            }
            catch (ItemNotMappedException)
            {
                MessageBox.Show(this, @"Working folder not mapped", @"Not mapped", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (tfsWorkspace.HasReadPermission) return true;
            MessageBox.Show(this, $@"{versionControl.AuthorizedUser} does not have read permission for {serverFolder}",
                @"Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        #endregion

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if (!Validate()) return;
            Clear();
            UseWait(s =>
            {
                btnCompile.Enabled = false;
                panel1.Show();
                Compile();
                btnCompile.Enabled = true;
            }, "Compiling now...");
        }

        private void UseWait(Action<string> action, string msg)
        {
            Cursor = Cursors.WaitCursor;
            panel1.Show();
            lblInfo.Text = msg;
            action(msg);
            panel1.Hide();
            Cursor = Cursors.Default;
        }

        private new bool Validate()
        {
            if (string.IsNullOrWhiteSpace(slnPath))
            {
                MessageBox.Show(this, @"Select the solution file", @"Required", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (File.Exists(slnPath)) return true;
            MessageBox.Show(this, @"Select the solution file", @"Not valid", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            return false;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            lstPreExtractFiles.Items.Clear();
            FilterAssemblies();
            FilterNamespaces();
        }

        private void FilterAssemblies()
        {
            var filteredAssemblies = cmbAssembly.CheckBoxItems.Where(x => x.Checked).ToList();
            if (!filteredAssemblies.Any()) return;
            foreach (var assembly in filteredAssemblies)
            {
                ReloadAssembly(assembly.Text);
            }
        }

        private void ReloadAssembly(string assemblyName)
        {
            foreach (ListViewItem item in lstBackup.Items)
            {
                if (item.SubItems[0].Text == assemblyName)
                {
                    ResetListView(item);
                }
            }
        }

        private void FilterNamespaces()
        {
            var filteredNamespace = cmbNamespace.CheckBoxItems.Where(x => x.Checked).ToList();
            if (!filteredNamespace.Any()) return;
            lstPreExtractFiles.Items.Clear();
            foreach (var @namespace in filteredNamespace)
            {
                ReloadNamespace(@namespace.Text);
            }
        }

        private void ReloadNamespace(string nameSpaceName)
        {
            foreach (ListViewItem item in lstBackup.Items)
            {
                if (item.SubItems[1].Text == nameSpaceName)
                {
                    ResetListView(item);
                }
            }
        }

        private void ResetListView(ListViewItem item)
        {
            var lvItem = new ListViewItem { Text = item.Text };
            lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = item.SubItems[1].Text });
            lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = item.SubItems[2].Text });
            lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = item.SubItems[3].Text });
            lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = item.SubItems[4].Text });
            lstPreExtractFiles.Items.Add(lvItem);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void cmbAssembly_CheckChanged(object sender, EventArgs e)
        {
            var filteredAssemblies = cmbAssembly.CheckBoxItems.Where(x => x.Checked).ToList();
            cmbNamespace.Items.Clear();
            var addedNamespaces = new List<string>();
            if (filteredAssemblies.Count > 0)
            {
                foreach (var @assembly in filteredAssemblies)
                {
                    foreach (ListViewItem item in lstBackup.Items)
                    {
                        if (item.SubItems[0].Text != @assembly.Text ||
                            addedNamespaces.Exists(x => x == item.SubItems[1].Text)) continue;
                        cmbNamespace.Items.Add(item.SubItems[1].Text);
                        addedNamespaces.Add(item.SubItems[1].Text);
                    }
                }
                cmbAssembly.Text = string.Join(",", filteredAssemblies.Select(x => x.Text));
            }
            else
            {
                foreach (ListViewItem item in lstBackup.Items)
                {
                    if (addedNamespaces.Exists(x => x == item.SubItems[1].Text)) continue;
                    cmbNamespace.Items.Add(item.SubItems[1].Text);
                    addedNamespaces.Add(item.SubItems[1].Text);
                }
                cmbAssembly.Text = string.Empty;
            }
        }

        private void cmbNamespace_CheckChanged(object sender, EventArgs e)
        {
            var filteredNamespaces = cmbNamespace.CheckBoxItems.Where(x => x.Checked).ToList();
            cmbNamespace.Text = filteredNamespaces.Any() ? string.Join(",", filteredNamespaces.Select(x => x.Text)) : string.Empty;
        }

        private static async Task<Microsoft.CodeAnalysis.Document> RemoveUnusedImportDirectivesAsync(Microsoft.CodeAnalysis.Document document, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            root = RemoveUnusedImportDirectives(semanticModel, root, cancellationToken);
            document = document.WithSyntaxRoot(root);
            return document;
        }

        private static SyntaxNode RemoveUnusedImportDirectives(SemanticModel semanticModel, SyntaxNode root, CancellationToken cancellationToken)
        {
            var oldUsings = root.DescendantNodesAndSelf().Where(s => s is UsingDirectiveSyntax);
            var unusedUsings = GetUnusedImportDirectives(semanticModel, cancellationToken);
            var leadingTrivia = root.GetLeadingTrivia();

            root = root.RemoveNodes(oldUsings, SyntaxRemoveOptions.KeepNoTrivia);
            var newUsings = SyntaxFactory.List(oldUsings.Except(unusedUsings));

            root = ((CompilationUnitSyntax)root)
                .WithUsings(newUsings)
                .WithLeadingTrivia(leadingTrivia);

            return root;
        }

        private static HashSet<SyntaxNode> GetUnusedImportDirectives(SemanticModel model, CancellationToken cancellationToken)
        {
            var unusedImportDirectives = new HashSet<SyntaxNode>();
            var root = model.SyntaxTree.GetRoot(cancellationToken);
            foreach (var diagnostic in model.GetDiagnostics(null, cancellationToken).Where(d => d.Id == "CS8019" || d.Id == "CS0105"))
            {
                var usingDirectiveSyntax = root.FindNode(diagnostic.Location.SourceSpan, false, false) as UsingDirectiveSyntax;
                if (usingDirectiveSyntax != null)
                {
                    unusedImportDirectives.Add(usingDirectiveSyntax);
                }
            }

            return unusedImportDirectives;
        }
    }

    internal class ProjectInfo
    {
        public string FilePath { get; set; }
        public string ClassFilePath { get; set; }
        public IEnumerable<SyntaxTree> SyntaxTrees { get; set; }
        public Project Project { get; set; }
        public CompilationUnitSyntax CompilationUnit { get; set; }
        public ProjectInfo()
        {
            SyntaxTrees = new List<SyntaxTree>();
        }

    }
}


internal class ListViewItemComparer : IComparer
{
    private readonly int col;
    private readonly SortOrder order;
    public ListViewItemComparer()
    {
        col = 0;
        order = SortOrder.Ascending;
    }
    public ListViewItemComparer(int column, SortOrder order)
    {
        col = column;
        this.order = order;
    }
    public int Compare(object x, object y)
    {
        var returnVal = string.CompareOrdinal(((ListViewItem)x).SubItems[col].Text,
            ((ListViewItem)y).SubItems[col].Text);
        // Determine whether the sort order is descending.
        if (order == SortOrder.Descending)
            // Invert the value returned by String.Compare.
            returnVal *= -1;
        return returnVal;
    }
}