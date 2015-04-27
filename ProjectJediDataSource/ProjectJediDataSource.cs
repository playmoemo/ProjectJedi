using DataModel;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1052:StaticHolderTypesShouldBeSealed"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class ProjectJediDataSource
    {
        private static DataContractJsonSerializerSettings jsonSerializerSettings;
        // It is used
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private static ProjectJediDataSource projectJediDataSource = new ProjectJediDataSource();
        private const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private const string RestServiceUrl = "http://localhost:22618/";
        private static Student admin;

        private ProjectJediDataSource()
        {

            jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
        }

        private static ObservableCollection<Group> groups = new ObservableCollection<Group>();
        public static ObservableCollection<Group> Groups { get { return ProjectJediDataSource.groups; } }

        private static ObservableCollection<Student> students = new ObservableCollection<Student>();
        public static ObservableCollection<Student> Students { get { return ProjectJediDataSource.students; } }

        private static  ObservableCollection<StudentTask> studentTasks = new ObservableCollection<StudentTask>();
        public static ObservableCollection<StudentTask> StudentTasks { get { return ProjectJediDataSource.studentTasks; } }

        private static ObservableCollection<TimeSheet> timeSheets = new ObservableCollection<TimeSheet>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheets")]
        public static ObservableCollection<TimeSheet> TimeSheets { get { return ProjectJediDataSource.timeSheets; } }

        private static ObservableCollection<StudentTask> criticalTasks = new ObservableCollection<StudentTask>();
        public static ObservableCollection<StudentTask> CriticalTasks { get { return ProjectJediDataSource.criticalTasks; } }

        /// <summary>
        /// Sets the administrator.
        /// </summary>
        /// <param name="student">The student.</param>
        public static void SetAdmin(Student student)
        {
            ProjectJediDataSource.admin = student;
        }

        /// <summary>
        /// Sets the HTTP client settings.
        /// </summary>
        /// <returns>The client</returns>
        private static HttpClient setHttpClientSettings()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(RestServiceUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


        /// <summary>
        /// Populates the local collections.
        /// </summary>
        public static async void PopulateLocalResources()
        {
            ProjectJediDataSource.groups = await ProjectJediDataSource.GetGroupsAsync();
            ProjectJediDataSource.students = await ProjectJediDataSource.GetStudentsAsync();
            ProjectJediDataSource.studentTasks = await ProjectJediDataSource.GetStudentTasksAsync();
            ProjectJediDataSource.timeSheets = await ProjectJediDataSource.GetTimeSheetsAsync();

            foreach (var st in ProjectJediDataSource.studentTasks)
	        {
                if (st.StudentId == admin.StudentId)
                {
                    ProjectJediDataSource.criticalTasks.Add(st);
                }
	        }
        }

        //======================================GROUP=========================================================================
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
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

                ProjectJediDataSource.groups.Remove(ProjectJediDataSource.groups.First(g => g.GroupId == newGroup.GroupId));
                ProjectJediDataSource.groups.Add(newGroup);
            }
        }


        public static async Task ObliterateGroupAsync(Group group)
        {
            using (var client = setHttpClientSettings())
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(Group), jsonSerializerSettings);

                var response = await client.DeleteAsync("api/Groups/" + group.GroupId);
                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.groups.Remove(ProjectJediDataSource.groups.First(g => g.GroupId == group.GroupId));
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asyc")]
        public static async Task PostGroupAsyc(Group group, Student creator)
        {
            group.Students.Add(creator);

            using (var client = setHttpClientSettings())
            {

                var jsonSerializer = new DataContractJsonSerializer(typeof(Group), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, group);
                stream.Position = 0;

                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/groups", content);

                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.groups.Add(group);
            }
        }

        //===============================================STUDENT==========================================================
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asyc")]
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

                ProjectJediDataSource.students.Add(student);
            }
        }

        
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

                ProjectJediDataSource.students.Remove(ProjectJediDataSource.students.First(s => s.StudentId == student.StudentId));
                ProjectJediDataSource.students.Add(student);
            }
        }

        
        public static async Task ObliterateStudentAsync(Student student)
        {
            using (var client = setHttpClientSettings())
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(Student), jsonSerializerSettings);

                var response = await client.DeleteAsync("api/Students/" + student.StudentId);
                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.students.Remove(ProjectJediDataSource.students.First(s => s.StudentId == student.StudentId));
            }
        }

        //===================================STUDENTTASK=====================================================================
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
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

        public static async Task PostStudentTaskAsync(StudentTask studentTask)
        {
            using (var client = setHttpClientSettings())
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(StudentTask), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, studentTask);
                stream.Position = 0;

                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/StudentTasks", content);

                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.studentTasks.Add(studentTask);
            }
        }

        public static async Task UpdateStudentTaskAsync(StudentTask studentTask)
        {
            using (var client = setHttpClientSettings())
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(StudentTask), jsonSerializerSettings);

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, studentTask);
                stream.Position = 0;
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/StudentTasks/" + studentTask.StudentTaskId, content);

                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.studentTasks.Remove(ProjectJediDataSource.studentTasks.First(st => st.StudentTaskId == studentTask.StudentTaskId));
                ProjectJediDataSource.studentTasks.Add(studentTask);
            }
        }

        
        public static async Task ObliterateStudentTaskAsync(StudentTask studentTask)
        {
            using (var client = setHttpClientSettings())
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(StudentTask), jsonSerializerSettings);

                var response = await client.DeleteAsync("api/StudentTasks/" + studentTask.StudentTaskId);
                response.EnsureSuccessStatusCode();

                ProjectJediDataSource.studentTasks.Remove(ProjectJediDataSource.studentTasks.First(st => st.StudentTaskId == studentTask.StudentTaskId));
            }
        }


        // ========================================TIMESHEET========================================================
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheets"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
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
    }
}
