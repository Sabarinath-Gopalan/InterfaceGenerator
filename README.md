# InterfaceGenerator

This tool was created using VS2015 with .net framework 4.6.2.  I have also used CheckboxComboBox from this path.

    https://www.codeproject.com/Articles/21085/CheckBox-ComboBox-Extending-the-ComboBox-Class-and


This tool will help you to extract interfaces for any of the classes found in your VS solution.  This will be useful if you want to migrate your existing solution to Interface style programming.
The tool allows you to select the solution and displays the list of assemblies, namespaces, classes and interfaces associated with it.
Optionally you can link with TFS source control as well.  It uses your windows authentication and your working folder.
After you extract, the interface files are added to the project by default.


This tool is still in early stages, the known issues at this point are as follows

    a. Doesn't support partial classes

    b. If more than one class inside a class file, the interface will extract for all the classes as seperate file however, only one of the class will implement.

    c. The clear button still doesn't clear the items in the listview.

