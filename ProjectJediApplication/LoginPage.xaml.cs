using DataModel;
using ProjectJediApplication.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class LoginPage : Page
    {
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private Boolean isLoggedIn = true;
       
        
        public LoginPage()
        {
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private async void btnSaveUserInformation_Click(object sender, RoutedEventArgs e)
        {
            /*
             * 1: POST a student for the current user
             * 2: save state for the CheckBox
             *      - check boolean value when app loads(DataLoadingPage.xaml.cs)[loadstate???]
             *      - Test roamingSettings hvis ikke current løsning funker!
             */
            //ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            

            //if (checkNotRemainLoggedIn.IsChecked.Equals(true))
            //{
            //    isLoggedIn = false;
            //}
            //else
            //{
            //    isLoggedIn = true;
            //}
            //roamingSettings.Values["automaticLogin"] = isLoggedIn;

            Student student = new Student() 
            { 
                FirstName = txtbFirstName.Text,
                LastName = txtbLastName.Text,
                UserName = txtbUserName.Text
            };

            // save the user to DB
            await ProjectJediDataSource.ProjectJediDataSource.PostStudentAsyc(student);

            this.Frame.Navigate(typeof(MainPage));
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
            if (checkNotRemainLoggedIn.IsChecked.Equals(true))
            {
                isLoggedIn = false;
            }
            else
            {
                isLoggedIn = true;
            }

            e.PageState["automaticLogin"] = isLoggedIn;
        }
    }
}
