using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectJediDataModel
{
    public class Student
    {
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

        // TODO: May add Email and phone!!!!!!!

        // Navigation Property
        public virtual ObservableCollection<Group> Groups { get; set; }
        public virtual ObservableCollection<StudentTask> StudentTasks { get; set; }
        public virtual ObservableCollection<TimeSheet> TimeSheets { get; set; }
    }

    public class Group
    {
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

        // Navigation Property
        public virtual ObservableCollection<Student> Students { get; set; }
        public virtual ObservableCollection<StudentTask> StudentTasks {get; set;}
    }

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

    public class TimeSheet
    {
        public TimeSheet()
        {
            //this.Student = new Student();
            //this.Group = new Group();
        }
        public int TimeSheetId { get; set; }

        [Required]
        [StringLength(100)]
        public string Activity { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public double Hours { get; set; }

        // Navigation Property
        //public Student Student { get; set; }
        // Foreign Key
        public int StudentId { get; set; }
        //public Group Group { get; set; }
        // Foreign Key
        public int GroupId { get; set; }
    }

    public class ProjectEntities : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }

        public ProjectEntities()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=oyvindt;User ID=oyvindt;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false; // Avoid cycles
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Group>()
                .HasMany<Student>(b => b.Students)
                .WithMany(a => a.Groups)
                .Map(ab =>
                {
                    ab.MapLeftKey("GroupId");
                    ab.MapRightKey("StudentId");
                    ab.ToTable("StudentGroup");
                });
        }
    }
}
