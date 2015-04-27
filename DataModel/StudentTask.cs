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
        public StudentTask() {}
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

        // Foreign Key
        public int StudentId { get; set; }

        // Foreign Key
        public int GroupId { get; set; }
    }
}
