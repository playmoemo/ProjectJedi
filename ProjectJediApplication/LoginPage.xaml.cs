using DataModel;
using ProjectJediApplication.Common;
using ProjectJediApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    /// The page used for login
    /// </summary>
    // I prefer Login
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
    public sealed partial class LoginPage : Page
    {
        private ParameterArguments arguments;
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        //private Boolean isLoggedIn = true;
       
        
        public LoginPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private async void btnSaveUserInformation_Click(object sender, RoutedEventArgs e)
        {
            string password = null;
            if (txtbFirstName.Text.Length != 0 || txtbLastName.Text.Length != 0 || txtbUserName.Text.Length != 0)
            {
                if (passwordPassword.Password.Equals(passwordConfirmPassword.Password))
                {
                    password = passwordPassword.Password;
                    createNewUser(password);
                }
                else
                {
                    MessageDialog passDialog = new MessageDialog("Passwords don't match. Try again.");
                    await passDialog.ShowAsync();
                }  
            }
            else
            {
                MessageDialog requiredDialog = new MessageDialog("You are missing some required fields. Try again.");
                await requiredDialog.ShowAsync();
            }
        }

        private async void createNewUser(string password)
        {
            Student student = new Student()
            {
                FirstName = txtbFirstName.Text,
                LastName = txtbLastName.Text,
                UserName = txtbUserName.Text,
                Email = txtbEmail.Text,
                Password = password
            };

            // save the user to DB
            await ProjectJediDataSource.ProjectJediDataSource.PostStudentAsyc(student);

            var students = await ProjectJediDataSource.ProjectJediDataSource.GetStudentsAsync();
            foreach (var s in students)
            {
                if (s.UserName.Equals(student.UserName))
                {
                    MessageDialog userExistsDialog = new MessageDialog("Your user is created. You will be automatically logged in now.");
                    await userExistsDialog.ShowAsync();

                    arguments = new ParameterArguments() { Administrator = s};
                    this.Frame.Navigate(typeof(MainPage), arguments);
                }
                else
                {
                    break;
                }
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string uName = txtbUserNameLogin.Text;
            string pass = passwordLogin.Password;

            ObservableCollection<Student> studentsFromDB = await ProjectJediDataSource.ProjectJediDataSource.GetStudentsAsync();
            List<Student> studentList = studentsFromDB.ToList<Student>();
            if (studentsFromDB.Count == 0)
            {
                MessageDialog createUserDialog = new MessageDialog("You need to create a user before you log in.");
                await createUserDialog.ShowAsync();

                txtbUserName.Text = txtbUserNameLogin.Text;
                txtbUserNameLogin.Text = "";
                passwordLogin.Password = "";
            }
            foreach (var s in studentsFromDB)
            {
                if (s.UserName.Equals(uName) && s.Password.Equals(pass))
                {
                    ProjectJediDataSource.ProjectJediDataSource.SetAdmin(s);
                    ProjectJediDataSource.ProjectJediDataSource.PopulateLocalResources();

                    arguments = new ParameterArguments() { Administrator = s};
                    this.Frame.Navigate(typeof(MainPage), arguments);
                    break;
                }
                else
                {
                    MessageDialog userMissingDialog = new MessageDialog("Username or Password was incorrect.");
                    await userMissingDialog.ShowAsync();

                    txtbUserNameLogin.Text = "";
                    passwordLogin.Password = "";
                    break;
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
           
        }

        // Did not have time to implement this
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e")]
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
        }
    }
}
