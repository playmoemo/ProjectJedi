using DataModel;
using ProjectJediApplication.Common;
using ProjectJediApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// The main page of the application.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ParameterArguments arguments;
        private Student admin;

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void populateCriticalTasksListBox()
        {
            listBoxNotifications.Items.Clear();
            listBoxNotifications.DisplayMemberPath = "StudentTaskName";
            foreach (var st in admin.StudentTasks)
            {
                listBoxNotifications.Items.Add(st);
            }   
        }

        private void BtnGroups_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsPage), arguments);
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StudentsPage), arguments);
        }


        private void BtnStudentTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StudentTasksPage), arguments);
        }

        private void BtnProgress_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProgressPage), arguments);
        }

        private void BtnTimeSheets_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TimeSheetsPage), arguments);
        }


        private void hyperChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            // Not yet implemented
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (null != e)
            {
                arguments = (ParameterArguments)e.Parameter;
                admin = arguments.Administrator;

                txtUserNameMainPage.Text = admin.UserName;
            }
            
            ProjectJediDataSource.ProjectJediDataSource.PopulateLocalResources();
            populateCriticalTasksListBox();
        }
    }
}
