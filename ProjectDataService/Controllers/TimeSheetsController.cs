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
    public class TimeSheetsController : ApiController
    {
        private ProjectJediContext db = new ProjectJediContext();

        // GET: api/TimeSheets
        public IQueryable<TimeSheet> GetTimeSheets()
        {
            return db.TimeSheets.Include(g => g.Activities);
        }

        // GET: api/TimeSheets/5
        [ResponseType(typeof(TimeSheet))]
        public async Task<IHttpActionResult> GetTimeSheet(int id)
        {
            TimeSheet timeSheet = await db.TimeSheets.FindAsync(id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            // Add Activities related to a TimeSheet
            await db.Entry(timeSheet).Collection(t => t.Activities).LoadAsync();

            return Ok(timeSheet);
        }

        // PUT: api/TimeSheets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTimeSheet(int id, TimeSheet timeSheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timeSheet.TimeSheetId)
            {
                return BadRequest();
            }

            db.Entry(timeSheet).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeSheetExists(id))
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

       

        // POST: api/TimeSheets
        [ResponseType(typeof(TimeSheet))]
        public async Task<IHttpActionResult> PostTimeSheet(TimeSheet timeSheet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TimeSheets.Add(timeSheet);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = timeSheet.TimeSheetId }, timeSheet);
        }

        // DELETE: api/TimeSheets/5
        [ResponseType(typeof(TimeSheet))]
        public async Task<IHttpActionResult> DeleteTimeSheet(int id)
        {
            TimeSheet timeSheet = await db.TimeSheets.FindAsync(id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            db.TimeSheets.Remove(timeSheet);
            await db.SaveChangesAsync();

            return Ok(timeSheet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimeSheetExists(int id)
        {
            return db.TimeSheets.Count(e => e.TimeSheetId == id) > 0;
        }
    }
}