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
using System.Runtime.Serialization;
using System.IO;

namespace ProjectJediDataSource
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class ProjectJediDataSource
    {
        //private static HttpClient client;
        private static DataContractJsonSerializerSettings jsonSerializerSettings;
        private static ProjectJediDataSource projectJediDataSource = new ProjectJediDataSource();
        private const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private const string RestServiceUrl = "http://localhost:22618/";

        private ProjectJediDataSource()
        {
            //client = new HttpClient();
            //client.BaseAddress = new Uri(RestServiceUrl);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
        }

        private ObservableCollection<Group> groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups { get { return this.groups; } }

        private ObservableCollection<Student> students = new ObservableCollection<Student>();
        public ObservableCollection<Student> Students { get { return this.students; } }

        private ObservableCollection<StudentTask> studentTasks = new ObservableCollection<StudentTask>();
        public ObservableCollection<StudentTask> StudentTasks { get { return this.studentTasks; } }

        private ObservableCollection<TimeSheet> timeSheets = new ObservableCollection<TimeSheet>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheets")]
        public ObservableCollection<TimeSheet> TimeSheets { get { return this.timeSheets; } }

        // StudentTasks with close deadline to be presented in Notification area
        private ObservableCollection<StudentTask> criticalTasks = new ObservableCollection<StudentTask>();
        public ObservableCollection<StudentTask> CriticalTasks { get { return this.criticalTasks; } }


       /*Lage forskjellige versjoner av disse metodene utifra hva som skal
        * vises på siden(f.eks i Notification area, hente StudentTasks på deadline)
        */
       

        private static HttpClient setHttpClientSettings()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(RestServiceUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        //======================================GROUP=========================================================================
        public static async Task<ObservableCollection<Group>> GetGroupsAsync()
        {
            using (var client = setHttpClientSettings())
            {
                var response = await client.GetAsync("api/Groups");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Group>), jsonSerializerSettings);
                    ObservableCollection<Group> groups = (ObservableCollection<Group>)serializer.ReadObject(result);

                    return groups;
                }
                else
                {
                    return null;
                }
            }
        }


        public static async Task UpdateGroupAsync(Group newGroup)
        {
            using (var client = setHttpClientSettings())
            {
                
                var jsonSerializer = new DataContractJsonSerializer(typeof(Group), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newGroup);
                stream.Position = 0;
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/groups/" + newGroup.GroupId, content);

                response.EnsureSuccessStatusCode();

                //ProjectJediDataSource.projectJediDataSource.groups.Remove(ProjectJediDataSource.projectJediDataSource.groups.First(g => g.GroupId == newGroup.GroupId));
                //ProjectJediDataSource.projectJediDataSource.groups.Add(newGroup);
            }
        }


        public static async Task PostGroupAsyc(Group group)
        {
            using (var client = setHttpClientSettings())
            {

                var jsonSerializer = new DataContractJsonSerializer(typeof(Group), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, group);
                stream.Position = 0;

                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/groups", content);

                response.EnsureSuccessStatusCode();
            }
        }

        //===============================================STUDENT==========================================================
        public static async Task<ObservableCollection<Student>> GetStudentsAsync()
        {
            using (var client = setHttpClientSettings())
            {
                
                var response = await client.GetAsync("api/Students");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Student>), jsonSerializerSettings);
                    ObservableCollection<Student> students = (ObservableCollection<Student>)serializer.ReadObject(result);

                    return students;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task PostStudentAsyc(Student student)
        {
            using (var client = setHttpClientSettings())
            {
                
                var jsonSerializer = new DataContractJsonSerializer(typeof(Student), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, student);
                stream.Position = 0;

                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/students", content);

                response.EnsureSuccessStatusCode();
            }
        }

        // Update a student with PUT
        public static async Task UpdateStudentAsync(Student student)
        {
            using (var client = setHttpClientSettings())
            {
                
                var jsonSerializer = new DataContractJsonSerializer(typeof(Student), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, student);
                stream.Position = 0;
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/students/" + student.StudentId, content);

                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.projectJediDataSource.students.Remove(ProjectJediDataSource.projectJediDataSource.students.First(s => s.StudentId == student.StudentId));
                ProjectJediDataSource.projectJediDataSource.students.Add(student);
            }
        }

        // Delete student with DELETE
        public static async Task ObliterateStudentAsync(Student student)
        {
            using (var client = setHttpClientSettings())
            {
                //client.BaseAddress = new Uri(RestServiceUrl);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonSerializer = new DataContractJsonSerializer(typeof(Student), jsonSerializerSettings);

                var response = await client.DeleteAsync("api/Students/" + student.StudentId);
                response.EnsureSuccessStatusCode();

                // update DataSource
                //ProjectJediDataSource.projectJediDataSource.students.Remove(ProjectJediDataSource.projectJediDataSource.students.First(s => s.StudentId == student.StudentId));
            }
        }

        //===================================STUDENTTASK=====================================================================
        public static async Task<ObservableCollection<StudentTask>> GetStudentTasksAsync()
        {
            using (var client = setHttpClientSettings())
            {
                
                var response = await client.GetAsync("api/StudentTasks");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<StudentTask>), jsonSerializerSettings);
                    ObservableCollection<StudentTask> studentTasks = (ObservableCollection<StudentTask>)serializer.ReadObject(result);

                    return studentTasks;
                }
                else
                {
                    return null;
                }
            }
        }

        //DELETE
        public static async Task ObliterateStudentTaskAsync(StudentTask studentTask)
        {
            using (var client = setHttpClientSettings())
            {

                var jsonSerializer = new DataContractJsonSerializer(typeof(StudentTask), jsonSerializerSettings);

                var response = await client.DeleteAsync("api/StudentTasks/" + studentTask.StudentTaskId);
                response.EnsureSuccessStatusCode();

                // update DataSource
                ProjectJediDataSource.projectJediDataSource.studentTasks.Remove(ProjectJediDataSource.projectJediDataSource.studentTasks.First(st => st.StudentTaskId == studentTask.StudentTaskId));
            }
        }


        // ========================================TIMESHEET========================================================
        public static async Task<ObservableCollection<TimeSheet>> GetTimeSheetsAsync()
        {
            using (var client = setHttpClientSettings())
            {
                
                var response = await client.GetAsync("api/Timesheets");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<TimeSheet>), jsonSerializerSettings);
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
