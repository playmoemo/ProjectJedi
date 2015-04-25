using ProjectJediApplication.Common;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ProjectJediApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataLoadingPage : Page
    {
        private Boolean willLoadLogin = true;

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

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            activateDataLoading();
            
            // retrieve state of "automaticLogin", and load/not load log-in page
        }

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
                
                if (willLoadLogin)
                {
                    this.Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                
            }
            else
            {
                progressLoadingData.IsActive = true;
                progressLoadingData.Visibility = Visibility.Visible;

                activateDataLoading();
            }
        }

        
        private async Task<Boolean> getDataFromDatabase()
        {
            try { 
                this.DefaultViewModel["Groups"] = await ProjectJediDataSource.ProjectJediDataSource.GetGroupsAsync();
                this.DefaultViewModel["Students"] = await ProjectJediDataSource.ProjectJediDataSource.GetStudentsAsync();
                this.DefaultViewModel["StudentTasks"] = await ProjectJediDataSource.ProjectJediDataSource.GetStudentTasksAsync();
                this.DefaultViewModel["TimeSheets"] = await ProjectJediDataSource.ProjectJediDataSource.GetTimeSheetsAsync();

                return true;

            } catch(Exception e) {
                //USE PROPER EXCEPTION HANDLING!!!
                e.ToString();
                return false;
            }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Restore the previously saved state associated with this page
            if (e.PageState != null && e.PageState.ContainsKey("automaticLogin"))
            {
                //ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                willLoadLogin = (Boolean)e.PageState["automaticLogin"];
            }
            //if (e.PageState == null)
            //{
                
            //}
            //else
            //{
            //    // Restore the previously saved state associated with this page
            //    if (e.PageState != null && e.PageState.ContainsKey("automaticLogin"))
            //    {
                    
            //    }
            //}
        }
    }
}
