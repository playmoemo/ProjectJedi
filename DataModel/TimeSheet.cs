using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheet")]
    public class TimeSheet
    {
        public TimeSheet()
        {
            this.Activities = new ObservableCollection<Activity>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheet")]
        public int TimeSheetId { get; set; }
        public String TimeSheetName { get; set; }
        
        // Navigation Property
        // Foreign Key
        public int StudentId { get; set; }
        // Foreign Key
        public int GroupId { get; set; }
        public virtual ObservableCollection<Activity> Activities { get; set; }
    }
}
