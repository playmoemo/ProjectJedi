using ProjectJediApplication.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public DataLoadingPage()
        {
            this.InitializeComponent();
            //txtCount.Text = "50";
            txtCount.Text = this.DefaultViewModel.Count().ToString();

            

            //getDataFromDatabase();
            activateDataLoading();
            //txtCount.Text = this.DefaultViewModel.Count().ToString();

            
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

                this.Frame.Navigate(typeof(MainPage));
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
                e.ToString();
                return false;
            }
            
            //if (this.DefaultViewModel.Count == 4)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
