using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            this.Students = new ObservableCollection<Student>();
            this.StudentTasks = new ObservableCollection<StudentTask>();
        }
        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }
        public string Description { get; set; }

        //må legge til denne
        // public int GroupLeader { get; set; } // StudentId of Student that creates Group....

        // Navigation Property
        //suppressed because of the way Entity Framework operates
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Student> Students { get; set; }

        //Suppressed because of how Entity Framework works
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<StudentTask> StudentTasks { get; set; }
    }
}
