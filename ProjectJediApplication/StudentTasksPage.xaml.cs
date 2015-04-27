using DataModel;
using ProjectJediApplication.Common;
using ProjectJediApplication.DataModel;
using System;
using System.Collections.Generic;
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
    ///The page that displays, creates and edit tasks 
    /// </summary>
    public sealed partial class StudentTasksPage : Page
    {
        private ParameterArguments arguments;
        private Student admin;
        private enum TaskStatus { New, Active, Finished };

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        public StudentTasksPage()
        {
            this.InitializeComponent();

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


            var studentTask = (StudentTask)this.itemListView.SelectedItem;
            
            datePickerDeadline.Date = studentTask.Deadline.Date;

            TaskStatus[] status = { TaskStatus.New, TaskStatus.Active, TaskStatus.Finished };
            comboStudentTaskStatus.Items.Clear();
            foreach (var s in status)
            {
                comboStudentTaskStatus.Items.Add(s);
            }
            comboStudentTaskStatus.SelectedIndex = studentTask.Status;
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
            this.DefaultViewModel["StudentTasks"] = await ProjectJediDataSource.ProjectJediDataSource.GetStudentTasksAsync();

            if (e.PageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.tasksViewSource.View != null)
                {
                    this.tasksViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (e.PageState.ContainsKey("SelectedItem") && this.tasksViewSource.View != null)
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
            if (this.tasksViewSource.View != null)
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
                var args = (ParameterArguments)e.Parameter;
                arguments = args;
                admin = args.Administrator;

                var taskToBeSelected = args.StudentTask;

                this.itemListView.SelectedItem = taskToBeSelected;
            }
            
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        // Bottom AppBar buttons
        private async void appBarDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            StudentTask studentTask = (StudentTask)this.itemListView.SelectedItem;

            await ProjectJediDataSource.ProjectJediDataSource.ObliterateStudentTaskAsync(studentTask);

            this.Frame.Navigate(typeof(StudentTasksPage), arguments);
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }


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


        private async void btnSaveTaskChanges_Click(object sender, RoutedEventArgs e)
        {
            var studentTask = (StudentTask)this.itemListView.SelectedItem;
            var taskName = txtbStudentTaskName.Text;
            var description = txtbStudentTaskDescription.Text;
            var studentId = int.Parse(txtbOwner.Text);
            
            string dateString = datePickerDeadline.Date.ToString();
            DateTime dateTime = datePickerDeadline.Date.DateTime;
            var status = (TaskStatus)comboStudentTaskStatus.SelectedValue;
            var updateStudentTask = new StudentTask() 
            {
                StudentTaskId = studentTask.StudentTaskId,
                StudentTaskName = taskName,
                Description = description,
                Deadline = dateTime,
                Status = (int)status,
                StudentId = studentId,
                GroupId = studentTask.GroupId
            };

            await ProjectJediDataSource.ProjectJediDataSource.UpdateStudentTaskAsync(updateStudentTask);
            this.Frame.Navigate(typeof(StudentTasksPage), arguments);
            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }

        private void btnCreateTask_Click(object sender, RoutedEventArgs e)
        {
            txtbStudentTaskName.Text = "";
            txtbStudentTaskDescription.Text = "";
            datePickerDeadline.Date = DateTime.Now;
            comboStudentTaskStatus.SelectedIndex = 0;
            txtbOwner.Text = admin.StudentId.ToString();

            ProjectJediDataSource.ProjectJediDataSource.PopulateLocalResources();
            var groups = ProjectJediDataSource.ProjectJediDataSource.Groups;
            var students = ProjectJediDataSource.ProjectJediDataSource.Students;

            listBoxPickGroup.Items.Clear();
            listBoxPickGroup.DisplayMemberPath = "GroupName";
            foreach (var group in groups)
            {
                listBoxPickGroup.Items.Add(group);
            }

            listBoxPickOwner.Items.Clear();
            listBoxPickOwner.DisplayMemberPath = "UserName";
            foreach (var student in students)
            {
                listBoxPickOwner.Items.Add(student);
            }

            txtbStudentTaskName.Focus(FocusState.Programmatic);
        }

        private async void btnSaveNewStudentTask_Click(object sender, RoutedEventArgs e)
        {
            if (txtbStudentTaskName.Text.Count() < 1 || txtbStudentTaskDescription.Text.Count() < 1)
            {
                MessageDialog missingInputDialog = new MessageDialog("You are missing some input.");
                await missingInputDialog.ShowAsync();
            }
            else
            {
                StudentTask studentTask = new StudentTask()
                { 
                    StudentTaskName = txtbStudentTaskName.Text,
                    Description = txtbStudentTaskDescription.Text,
                    Deadline = datePickerDeadline.Date.DateTime,
                    Status = comboStudentTaskStatus.SelectedIndex,
                    StudentId = int.Parse(txtbOwner.Text),
                    GroupId = int.Parse(txtbStudentTaskGroup.Text)
                };

                await ProjectJediDataSource.ProjectJediDataSource.PostStudentTaskAsync(studentTask);

                this.Frame.Navigate(typeof(StudentTasksPage), arguments);
                Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            }
        }

        private void listBoxPickGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group group = (Group)listBoxPickGroup.SelectedItem;
            txtbStudentTaskGroup.Text = group.GroupId.ToString();
        }

        private void listBoxPickOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Student student = (Student)listBoxPickOwner.SelectedItem;
            txtbOwner.Text = student.StudentId.ToString();
        }
    }
}
