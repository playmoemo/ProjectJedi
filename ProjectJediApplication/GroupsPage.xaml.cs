using DataModel;
using ProjectJediApplication.Common;
using ProjectJediApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ProjectJediApplication
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class GroupsPage : Page
    {
        private Student admin;
        private ParameterArguments arguments;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public GroupsPage()
        {
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // Setup the logical page navigation components that allow
            // the page to only show one pane at a time.
            this.navigationHelper.GoBackCommand = new ProjectJediApplication.Common.RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            this.itemListView.SelectionChanged += itemListView_SelectionChanged;

            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation())
            {
                this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
            }            
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.DefaultViewModel["Groups"] = await ProjectJediDataSource.ProjectJediDataSource.GetGroupsAsync();

            if (e.PageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.groupsViewSource.View != null)
                {
                    this.groupsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (e.PageState.ContainsKey("SelectedItem") && this.groupsViewSource.View != null)
                {
                    // TODO: Invoke Me.itemsViewSource.View.MoveCurrentTo() with the selected
                    //       item as specified by the value of pageState("SelectedItem")

                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.groupsViewSource.View != null)
            {
                // TODO: Derive a serializable navigation parameter and assign it to
                //       pageState("SelectedItem")

            }
        }

        #region Logical page navigation

        // The split page isdesigned so that when the Window does have enough space to show
        // both the list and the dteails, only one pane will be shown at at time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <returns>True if the window should show act as one logical page, false
        /// otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Invoked with the Window changes size
        /// </summary>
        /// <param name="sender">The current Window</param>
        /// <param name="e">Event data that describes the new size of the Window</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();

            // Populate ListBox with StudentTasks
            var group = (Group)this.itemListView.SelectedItem;
            
            listBoxStudentTasks.Items.Clear();
            listBoxStudentTasks.DisplayMemberPath = "StudentTaskName";
            foreach (var st in group.StudentTasks)
            {
                listBoxStudentTasks.Items.Add(st);
            }

            // Populate ListBox with Students
            listBoxStudents.Items.Clear();
            listBoxStudents.DisplayMemberPath = "UserName";
            foreach (var s in group.Students)
            {
                listBoxStudents.Items.Add(s);
            }

            // Update ProjectJediDataSource against DB
            ProjectJediDataSource.ProjectJediDataSource.PopulateLocalResources();
            // 
            var allStudents = ProjectJediDataSource.ProjectJediDataSource.Students;
            var studentList = allStudents.ToList();
            var studentsInGroup = group.Students;
            
            listBoxStudentsToAdd.Items.Clear();
            listBoxStudentsToAdd.DisplayMemberPath = "UserName";
            
            foreach (var student in studentList)
            {
                foreach (var s in studentsInGroup)
                {
                    if (student.StudentId == s.StudentId)
                    {
                        allStudents.Remove(student);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            foreach (var student in allStudents)
            {
                listBoxStudentsToAdd.Items.Add(student);
            }
            
        }

        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return to
                // the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (null != e)
            {
                arguments = (ParameterArguments)e.Parameter;
                admin = arguments.Administrator;

                navigationHelper.OnNavigatedTo(e);
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        // Top AppBar buttons
        private void appBarNavHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), arguments);
        }

        private void appBarNavStudents_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StudentsPage), arguments);
        }

        private void appBarNavTimeSheets_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TimeSheetsPage), arguments);
        }

        private void appBarNavTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StudentTasksPage), arguments);
        }

        private void appBarNavGroups_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsPage), arguments);
        }



        // PUT Group
        private async void btnSaveGroupChanges_Click(object sender, RoutedEventArgs e)
        {
            var group = (Group)this.itemListView.SelectedItem;
            var groupName = txtbGroupName.Text;
            var groupDescription = txtbGroupDescription.Text;
            var newGroup = new Group() { GroupId = group.GroupId, GroupName = groupName, Description = groupDescription, Students = group.Students, StudentTasks = group.StudentTasks};
            
            await ProjectJediDataSource.ProjectJediDataSource.UpdateGroupAsync(newGroup);

            this.Frame.Navigate(typeof(GroupsPage), arguments);
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }




        // Bottom AppBar buttons
        private async void appBarDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            var group = (Group)itemListView.SelectedItem;

            if (group.GroupLeader == admin.StudentId)
            {
                MessageDialog notAuthorizedDialog = new MessageDialog("You will now delete the group.");
                await notAuthorizedDialog.ShowAsync();

                await ProjectJediDataSource.ProjectJediDataSource.ObliterateGroupAsync(group);

                this.Frame.Navigate(typeof(GroupsPage), arguments);
                Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            }
            else
            {
                MessageDialog notAuthorizedDialog = new MessageDialog("You are not authorized to delete a group.");
                await notAuthorizedDialog.ShowAsync();
            }            
        }

       
        private void btnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            // clear input fields
            txtbGroupName.Text = "";
            txtbGroupDescription.Text = "";
            txtbGroupLeader.Text = admin.StudentId.ToString();
            listBoxStudentTasks.Items.Clear();
            listBoxStudents.Items.Clear();

            txtbGroupName.Focus(FocusState.Programmatic);

        }

        private async void btnSaveNewGroup_Click(object sender, RoutedEventArgs e)
        {
            if (txtbGroupName.Text.Count() < 1 || txtbGroupLeader.Text.Count() < 1)
            {
                MessageDialog missingInputDialog = new MessageDialog("You are missing some input.");
                await missingInputDialog.ShowAsync();
            }
            else
            {
                Group group = new Group() { GroupName = txtbGroupName.Text, Description = txtbGroupDescription.Text, GroupLeader = admin.StudentId};
                await ProjectJediDataSource.ProjectJediDataSource.PostGroupAsyc(group, admin);

                this.Frame.Navigate(typeof(GroupsPage), arguments);
                Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            }
        }

        private void listBoxStudentTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Navigate to StudentTasksPage with the selected StudentTask as an argument
            StudentTask studentTask = (StudentTask)listBoxStudentTasks.SelectedItem;
            ParameterArguments args = new ParameterArguments() {StudentTask = studentTask, Administrator = admin };
            this.Frame.Navigate(typeof(StudentTasksPage), args);
        }

        private void listBoxStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Navigate to StudentsPage with the selected Student as a part of the argument
            Student student = (Student)listBoxStudents.SelectedItem;
            ParameterArguments args = new ParameterArguments() { Student = student, Administrator = admin };
            this.Frame.Navigate(typeof(StudentsPage), args);
        }

        private async void listBoxStudentsToAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add selected Student to the currently selected Group and save changes
            Student student = (Student)listBoxStudentsToAdd.SelectedItem;
            ParameterArguments args = new ParameterArguments() { Student = student, Administrator = admin};

            var group = (Group)this.itemListView.SelectedItem;
            group.Students.Add(student);
            
            await ProjectJediDataSource.ProjectJediDataSource.UpdateGroupAsync(group);
            ProjectJediDataSource.ProjectJediDataSource.PopulateLocalResources();

            this.Frame.Navigate(typeof(GroupsPage), arguments);
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }
    }
}
