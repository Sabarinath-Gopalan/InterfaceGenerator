using PresentationControls;

namespace InterfaceGenerator
{
    partial class FrmInterfaceGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelectSlnFolder = new System.Windows.Forms.TextBox();
            this.btnSelectSlnFolder = new System.Windows.Forms.Button();
            this.btnGetFiles = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lstPreExtractFiles = new System.Windows.Forms.ListView();
            this.colAssembly = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNamespace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInterface = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.slnFileOpen = new System.Windows.Forms.OpenFileDialog();
            this.btnCompile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.chkSubFolder = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbAssembly = new PresentationControls.CheckBoxComboBox();
            this.cmbNamespace = new PresentationControls.CheckBoxComboBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select .sln File:";
            // 
            // txtSelectSlnFolder
            // 
            this.txtSelectSlnFolder.Location = new System.Drawing.Point(110, 16);
            this.txtSelectSlnFolder.Name = "txtSelectSlnFolder";
            this.txtSelectSlnFolder.Size = new System.Drawing.Size(580, 20);
            this.txtSelectSlnFolder.TabIndex = 1;
            // 
            // btnSelectSlnFolder
            // 
            this.btnSelectSlnFolder.Location = new System.Drawing.Point(696, 16);
            this.btnSelectSlnFolder.Name = "btnSelectSlnFolder";
            this.btnSelectSlnFolder.Size = new System.Drawing.Size(75, 21);
            this.btnSelectSlnFolder.TabIndex = 2;
            this.btnSelectSlnFolder.Text = "&Select";
            this.btnSelectSlnFolder.UseVisualStyleBackColor = true;
            this.btnSelectSlnFolder.Click += new System.EventHandler(this.btnSelectSlnFolder_Click);
            // 
            // btnGetFiles
            // 
            this.btnGetFiles.Location = new System.Drawing.Point(110, 42);
            this.btnGetFiles.Name = "btnGetFiles";
            this.btnGetFiles.Size = new System.Drawing.Size(75, 23);
            this.btnGetFiles.TabIndex = 6;
            this.btnGetFiles.Text = "Get &Latest";
            this.btnGetFiles.UseVisualStyleBackColor = true;
            this.btnGetFiles.Click += new System.EventHandler(this.btnGetFiles_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lstPreExtractFiles);
            this.groupBox1.Location = new System.Drawing.Point(8, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 426);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extraction Types";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.lblInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 407);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(40, 179);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(652, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.AutoSize = true;
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblInfo.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lblInfo.Location = new System.Drawing.Point(35, 138);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(490, 20);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Please wait while we get assemblies, namespaces && classes";
            // 
            // lstPreExtractFiles
            // 
            this.lstPreExtractFiles.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.lstPreExtractFiles.CheckBoxes = true;
            this.lstPreExtractFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssembly,
            this.colNamespace,
            this.colClass,
            this.colInterface,
            this.colIFilePath});
            this.lstPreExtractFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPreExtractFiles.FullRowSelect = true;
            this.lstPreExtractFiles.GridLines = true;
            this.lstPreExtractFiles.Location = new System.Drawing.Point(3, 16);
            this.lstPreExtractFiles.Name = "lstPreExtractFiles";
            this.lstPreExtractFiles.Size = new System.Drawing.Size(755, 407);
            this.lstPreExtractFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstPreExtractFiles.TabIndex = 0;
            this.lstPreExtractFiles.UseCompatibleStateImageBehavior = false;
            this.lstPreExtractFiles.View = System.Windows.Forms.View.Details;
            this.lstPreExtractFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstPreExtractFiles_ColumnClick);
            // 
            // colAssembly
            // 
            this.colAssembly.Text = "Assembly name";
            this.colAssembly.Width = 160;
            // 
            // colNamespace
            // 
            this.colNamespace.Text = "Namespace";
            this.colNamespace.Width = 160;
            // 
            // colClass
            // 
            this.colClass.Text = "Class";
            this.colClass.Width = 120;
            // 
            // colInterface
            // 
            this.colInterface.Text = "Interface";
            // 
            // colIFilePath
            // 
            this.colIFilePath.Text = "File Path";
            // 
            // slnFileOpen
            // 
            this.slnFileOpen.Filter = "Solution File|*.sln";
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(191, 42);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(72, 23);
            this.btnCompile.TabIndex = 10;
            this.btnCompile.Text = "&Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Filter by Assembly";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Filter by Namespace";
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(696, 81);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 17;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(589, 139);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(103, 23);
            this.btnExtract.TabIndex = 18;
            this.btnExtract.Text = "Extract &Interface";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // chkSubFolder
            // 
            this.chkSubFolder.AutoSize = true;
            this.chkSubFolder.Location = new System.Drawing.Point(445, 145);
            this.chkSubFolder.Name = "chkSubFolder";
            this.chkSubFolder.Size = new System.Drawing.Size(138, 17);
            this.chkSubFolder.TabIndex = 19;
            this.chkSubFolder.Text = "Use separate sub-folder";
            this.chkSubFolder.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(697, 111);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 20;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbAssembly
            // 
            this.cmbAssembly.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAssembly.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            checkBoxProperties1.AutoEllipsis = true;
            checkBoxProperties1.AutoSize = true;
            checkBoxProperties1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAssembly.CheckBoxProperties = checkBoxProperties1;
            this.cmbAssembly.DisplayMemberSingleItem = "";
            this.cmbAssembly.FormattingEnabled = true;
            this.cmbAssembly.Location = new System.Drawing.Point(115, 82);
            this.cmbAssembly.MaxDropDownItems = 20;
            this.cmbAssembly.Name = "cmbAssembly";
            this.cmbAssembly.Size = new System.Drawing.Size(575, 21);
            this.cmbAssembly.TabIndex = 21;
            this.cmbAssembly.CheckBoxCheckedChanged += new System.EventHandler(this.cmbAssembly_CheckChanged);
            // 
            // cmbNamespace
            // 
            this.cmbNamespace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbNamespace.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            checkBoxProperties2.AutoEllipsis = true;
            checkBoxProperties2.AutoSize = true;
            checkBoxProperties2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbNamespace.CheckBoxProperties = checkBoxProperties2;
            this.cmbNamespace.DisplayMemberSingleItem = "";
            this.cmbNamespace.FormattingEnabled = true;
            this.cmbNamespace.Location = new System.Drawing.Point(115, 112);
            this.cmbNamespace.MaxDropDownItems = 10;
            this.cmbNamespace.Name = "cmbNamespace";
            this.cmbNamespace.Size = new System.Drawing.Size(577, 21);
            this.cmbNamespace.TabIndex = 22;
            this.cmbNamespace.CheckBoxCheckedChanged += new System.EventHandler(this.cmbNamespace_CheckChanged);
            // 
            // FrmInterfaceGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 598);
            this.Controls.Add(this.cmbNamespace);
            this.Controls.Add(this.cmbAssembly);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkSubFolder);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetFiles);
            this.Controls.Add(this.btnSelectSlnFolder);
            this.Controls.Add(this.txtSelectSlnFolder);
            this.Controls.Add(this.label1);
            this.Name = "FrmInterfaceGenerator";
            this.ShowIcon = false;
            this.Text = "Interface Generator";
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelectSlnFolder;
        private System.Windows.Forms.Button btnSelectSlnFolder;
        private System.Windows.Forms.Button btnGetFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lstPreExtractFiles;
        private System.Windows.Forms.ColumnHeader colAssembly;
        private System.Windows.Forms.OpenFileDialog slnFileOpen;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.ColumnHeader colNamespace;
        private System.Windows.Forms.ColumnHeader colClass;
        
        private System.Windows.Forms.Label label2;
        
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.ColumnHeader colInterface;
        private System.Windows.Forms.CheckBox chkSubFolder;
        private System.Windows.Forms.ColumnHeader colIFilePath;
        private System.Windows.Forms.Button btnClear;
        private CheckBoxComboBox cmbAssembly;
        private CheckBoxComboBox cmbNamespace;
    }
}

