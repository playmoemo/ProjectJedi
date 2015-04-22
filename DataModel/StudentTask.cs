using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class StudentTask
    {
        // går det an å kun ha id fra student og gruppe?
        // StudentTask object må inneholde Student og Group object når den legges til i db...
        public StudentTask()
        {
            //this.Student = new Student();
            //this.Group = new Group();
        }
        public int StudentTaskId { get; set; }

        [Required]
        [StringLength(50)]
        public String StudentTaskName { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public int Status { get; set; }

        // Navigation Property
        //public Student Student { get; set; }
        // Foreign Key
        public int StudentId { get; set; }

        //public Group Group { get; set; }
        // Foreign Key
        public int GroupId { get; set; }
    }
}
