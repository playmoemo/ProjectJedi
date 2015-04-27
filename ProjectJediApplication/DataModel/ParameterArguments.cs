using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectJediApplication.DataModel
{
    public class ParameterArguments
    {
        public Student Administrator { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public StudentTask StudentTask { get; set; }
        public TimeSheet TimeSheet { get; set; }
    }
}
