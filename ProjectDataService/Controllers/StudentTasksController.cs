using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess;
using DataModel;

namespace ProjectDataService.Controllers
{
    public class StudentTasksController : ApiController
    {
        private ProjectJediContext db = new ProjectJediContext();

        // GET: api/StudentTasks
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IQueryable<StudentTask> GetStudentTasks()
        {
            return db.StudentTasks;
        }

        // GET: api/StudentTasks/5
        [ResponseType(typeof(StudentTask))]
        public async Task<IHttpActionResult> GetStudentTask(int id)
        {
            StudentTask studentTask = await db.StudentTasks.FindAsync(id);
            if (studentTask == null)
            {
                return NotFound();
            }

            return Ok(studentTask);
        }

        // PUT: api/StudentTasks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudentTask(int id, StudentTask studentTask)
        {
            if (!ModelState.IsValid || id != studentTask.StudentTaskId)
            {
                return BadRequest(ModelState);
            }

            //if (id != studentTask.StudentTaskId)
            //{
            //    return BadRequest();
            //}

            db.Entry(studentTask).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StudentTasks
        [ResponseType(typeof(StudentTask))]
        public async Task<IHttpActionResult> PostStudentTask(StudentTask studentTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // må ha en Student og en Group i studentTask

            var group = db.Groups.Find(studentTask.GroupId);
            // check if the StudentTask exists
            //var studentTasks = group.StudentTasks.ToList<StudentTask>();
            //foreach (var st in studentTasks) {
            //    if (studentTask.StudentTaskId == st.StudentTaskId)
            //    {
            //        return without adding to db
            //    }
            //}

            group.StudentTasks.Add(studentTask); // kanskje ikke slik???
            db.StudentTasks.Add(studentTask);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = studentTask.StudentTaskId }, studentTask);
        }

        // DELETE: api/StudentTasks/5
        [ResponseType(typeof(StudentTask))]
        public async Task<IHttpActionResult> DeleteStudentTask(int id)
        {
            StudentTask studentTask = await db.StudentTasks.FindAsync(id);
            if (studentTask == null)
            {
                return NotFound();
            }

            db.StudentTasks.Remove(studentTask);
            await db.SaveChangesAsync();

            return Ok(studentTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentTaskExists(int id)
        {
            return db.StudentTasks.Count(e => e.StudentTaskId == id) > 0;
        }
    }
}