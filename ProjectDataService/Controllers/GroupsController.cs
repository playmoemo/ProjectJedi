﻿using System;
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
    public class GroupsController : ApiController
    {
        private ProjectJediContext db = new ProjectJediContext();

        // GET: api/Groups
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IQueryable<Group> GetGroups()
        {
            return db.Groups.Include(g => g.Students).Include(g => g.StudentTasks);
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            await db.Entry(group).Collection(g => g.Students).LoadAsync();
            await db.Entry(group).Collection(g => g.StudentTasks).LoadAsync();

            return Ok(group);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid || id != group.GroupId)
            {
                return BadRequest(ModelState);
            }

            var oldGroup = db.Groups.Find(group.GroupId);

            oldGroup.Students.Clear();
            oldGroup.StudentTasks.Clear();

            oldGroup.GroupName = group.GroupName;
            oldGroup.Description = group.Description;

            db.Entry(oldGroup).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            var students = group.Students.ToList<Student>();
            group.Students.Clear();

            foreach (var s in students)
            {
                Student student = await db.Students.FindAsync(s.StudentId);
                group.Students.Add(student);
            }

            db.Groups.Add(group);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = group.GroupId }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> DeleteGroup(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
            await db.SaveChangesAsync();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.GroupId == id) > 0;
        }
    }
}