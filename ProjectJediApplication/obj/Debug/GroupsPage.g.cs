﻿

#pragma checksum "E:\VSProjects\ProjectJediSolution\ProjectJediApplication\GroupsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F34600AE63DCEE9121EECF0DA20FB332"
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
    partial class GroupsPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 33 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavHome_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 34 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavGroups_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 35 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavStudents_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 36 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavTasks_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 37 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarNavTimeSheets_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 86 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ItemListView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 181 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSaveGroupChanges_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 187 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.listBoxStudentsToAdd_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 183 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnCreateGroup_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 184 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSaveNewGroup_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 179 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.listBoxStudents_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 175 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.listBoxStudentTasks_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 13:
                #line 254 "..\..\GroupsPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.appBarDeleteGroup_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


