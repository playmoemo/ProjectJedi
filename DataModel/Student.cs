using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Groups = new ObservableCollection<Group>();
            this.StudentTasks = new ObservableCollection<StudentTask>();
            this.TimeSheets = new ObservableCollection<TimeSheet>();
        }
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        // TODO: May add Email

        // Navigation Property
        //suppressed because of how Entity Framework operates
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Group> Groups { get; set; }
        //suppressed because of how Entity Framework operates
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<StudentTask> StudentTasks { get; set; }
        //suppressed because of how Entity Framework operates
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<TimeSheet> TimeSheets { get; set; }
    }
}
