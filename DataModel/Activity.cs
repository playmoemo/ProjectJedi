using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Activity
    {
        // may need to add a constructor?
        public int ActivityId { get; set; }
        public String ActivityName { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public double Hours { get; set; }

        // Foreign Key
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheet")]
        public int TimeSheetId { get; set; }
    }
}
