using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectJediApplication.DataModel
{
    // this is only for design-time data
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    class SampleDataSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ObservableCollection<Group> Groups
        {
            get { return null; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ObservableCollection<Student> Students
        {
            get { return null; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ObservableCollection<StudentTask> StudentTasks
        {
            get { return null; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ObservableCollection<TimeSheet> TimeSheets
        {
            get { return null; }
        }

        public ObservableCollection<Activity> Activities
        {
            get { return null; }
        }
    }
}
