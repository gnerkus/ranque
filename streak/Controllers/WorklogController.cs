using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using streak.Models;

namespace streak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorklogController : ControllerBase
    {
        private readonly WorklogContext _context;

        public WorklogController(WorklogContext context)
        {
            _context = context;
        }

        // GET: api/Worklog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorklogItem>>> GetWorklogItems()
        {
            return await _context.WorklogItems.ToListAsync();
        }

        // GET: api/Worklog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorklogItem>> GetWorklogItem(long id)
        {
            var worklogItem = await _context.WorklogItems.FindAsync(id);

            if (worklogItem == null)
            {
                return NotFound();
            }

            return worklogItem;
        }

        // PUT: api/Worklog/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorklogItem(long id, WorklogItem worklogItem)
        {
            if (id != worklogItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(worklogItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorklogItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Worklog
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorklogItem>> PostWorklogItem(WorklogItem worklogItem)
        {
            _context.WorklogItems.Add(worklogItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorklogItem", new { id = worklogItem.Id }, worklogItem);
        }

        // DELETE: api/Worklog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorklogItem(long id)
        {
            var worklogItem = await _context.WorklogItems.FindAsync(id);
            if (worklogItem == null)
            {
                return NotFound();
            }

            _context.WorklogItems.Remove(worklogItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorklogItemExists(long id)
        {
            return _context.WorklogItems.Any(e => e.Id == id);
        }
    }
}
