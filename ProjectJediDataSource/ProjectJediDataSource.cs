﻿using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

namespace ProjectJediDataSource
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class ProjectJediDataSource
    {
        private const string RestServiceUrl = "http://localhost:22618/";// may need to add api/

        //private static ProjectJediDataSource projectJediDataSource = new ProjectJediDataSource();

        private ObservableCollection<Group> groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups { get { return this.groups; } }

        private ObservableCollection<Student> students = new ObservableCollection<Student>();
        public ObservableCollection<Student> Students { get { return this.students; } }

        private ObservableCollection<StudentTask> studentTasks = new ObservableCollection<StudentTask>();
        public ObservableCollection<StudentTask> StudentTasks { get { return this.studentTasks; } }

        private ObservableCollection<TimeSheet> timeSheets = new ObservableCollection<TimeSheet>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheets")]
        public ObservableCollection<TimeSheet> TimeSheets { get { return this.timeSheets; } }


       
        public static async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RestServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/Groups");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Group>));
                    ObservableCollection<Group> groups = (ObservableCollection<Group>)serializer.ReadObject(result);

                    return groups;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<Student>> GetStudentsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RestServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/Students");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Student>));
                    ObservableCollection<Student> students = (ObservableCollection<Student>)serializer.ReadObject(result);

                    return students;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<StudentTask>> GetStudentTasksAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RestServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/StudentTasks");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<StudentTask>));
                    ObservableCollection<StudentTask> studentTasks = (ObservableCollection<StudentTask>)serializer.ReadObject(result);

                    return studentTasks;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<TimeSheet>> GetTimeSheetsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RestServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/Timesheets");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<TimeSheet>));
                    ObservableCollection<TimeSheet> timeSheets = (ObservableCollection<TimeSheet>)serializer.ReadObject(result);

                    return timeSheets;
                }
                else
                {
                    return null;
                }
            }
        }
        //Todo: create methods for all the other stuff I need...
    }
}
