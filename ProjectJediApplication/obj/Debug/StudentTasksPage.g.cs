﻿

#pragma checksum "E:\VSProjects\ProjectJediSolution\ProjectJediApplication\StudentTasksPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "496F453971DC9F1EEA97A15302F96BBA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectJediApplication
{
    partial class StudentTasksPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 30 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavHome_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 31 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavGroups_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 32 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavStudents_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 33 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavTasks_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 34 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavTimeSheets_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 81 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ItemListView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 178 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnCreateTask_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 179 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSaveTaskChanges_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 243 "..\..\StudentTasksPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarDeleteTask_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


