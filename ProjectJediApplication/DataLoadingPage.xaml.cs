using DataModel;
using ProjectJediApplication.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    /// The page that loads data from DB on application start
    /// </summary>
    public sealed partial class DataLoadingPage : Page
    {
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

        public DataLoadingPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Activates the data loading.
        /// </summary>
        private async void activateDataLoading()
        {
            progressLoadingData.IsActive = true;
            progressLoadingData.Visibility = Visibility.Visible;

            Boolean success;
            success = await getDataFromDatabase();
            if (success)
            {
                progressLoadingData.IsActive = false;
                progressLoadingData.Visibility = Visibility.Collapsed;
                
                this.Frame.Navigate(typeof(LoginPage));

            }
            else
            {
                progressLoadingData.IsActive = true;
                progressLoadingData.Visibility = Visibility.Visible;

                activateDataLoading();
            }
        }


        /// <summary>
        /// Gets the data from database.
        /// </summary>
        /// <returns>A boolean value true or false</returns>
        private async Task<Boolean> getDataFromDatabase()
        {
            try { 
                this.DefaultViewModel["Groups"] = await ProjectJediDataSource.ProjectJediDataSource.GetGroupsAsync();
                this.DefaultViewModel["Students"] = await ProjectJediDataSource.ProjectJediDataSource.GetStudentsAsync();
                this.DefaultViewModel["StudentTasks"] = await ProjectJediDataSource.ProjectJediDataSource.GetStudentTasksAsync();
                this.DefaultViewModel["TimeSheets"] = await ProjectJediDataSource.ProjectJediDataSource.GetTimeSheetsAsync();

                return true;

            } catch(TimeoutException) {
                return false;
                throw;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            activateDataLoading();
            navigationHelper.OnNavigatedTo(e);
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("automaticLogin"))
            {
                
            }
        }
    }
}
