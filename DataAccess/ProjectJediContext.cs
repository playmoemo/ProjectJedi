using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProjectJediContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheets")]
        public DbSet<TimeSheet> TimeSheets { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public ProjectJediContext()
            : base(@"Data Source=donau.hiof.no;Initial Catalog=oyvindt;User ID=oyvindt;Password=Sommer15")
        {
            this.Configuration.ProxyCreationEnabled = false; // Avoid cycles
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (null != modelBuilder)
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
}
