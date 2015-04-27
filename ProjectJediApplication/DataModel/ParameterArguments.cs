using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectJediApplication.DataModel
{
    /// <summary>
    /// This class is used as argument when navigating between the different pages.
    /// </summary>
    public class ParameterArguments
    {
        public Student Administrator { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public StudentTask StudentTask { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheet")]
        public TimeSheet TimeSheet { get; set; }
    }
}
