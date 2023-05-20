using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class StatusController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public StatusController(PilotageDBContext context)
        {
            _context = context;
        }

       // // GET: api/Status
       // [HttpGet]
       // public async Task<ActionResult<IEnumerable<Status>>> GetStatus()
       // {
       //     return await _context.Status.ToListAsync();
       // }

       // // GET: api/Status/5
       // [HttpGet("{id}")]
       // public async Task<ActionResult<Status>> GetStatus(string id)
       // {
       //     var status = await _context.Status.FindAsync(id);

       //     if (status == null)
       //     {
       //         return NotFound();
       //     }

       //     return status;
       // }

       // // PUT: api/Status/5
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost("Update/{id}")]
       // public async Task<IActionResult> PutStatus(string id, Status status)
       // {
       //     if (id != status.IdStatus)
       //     {
       //         return BadRequest();
       //     }

       //     _context.Entry(status).State = EntityState.Modified;

       //     try
       //     {
       //         await _context.SaveChangesAsync();
       //     }
       //     catch (DbUpdateConcurrencyException)
       //     {
       //         if (!StatusExists(id))
       //         {
       //             return NotFound();
       //         }
       //         else
       //         {
       //             throw;
       //         }
       //     }

       //     return NoContent();
       // }

       // // POST: api/Status
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost]
       // public async Task<ActionResult<Status>> PostStatus(Status status)
       // {
       //     _context.Status.Add(status);
       //     await _context.SaveChangesAsync();

       //     return CreatedAtAction("GetStatus", new { id = status.IdStatus }, status);
       // }

       // // DELETE: api/Status/5
       //[HttpPost("Delete/{id}")]
       // public async Task<ActionResult<Status>> DeleteStatus(string id)
       // {
       //     var status = await _context.Status.FindAsync(id);
       //     if (status == null)
       //     {
       //         return NotFound();
       //     }

       //     _context.Status.Remove(status);
       //     await _context.SaveChangesAsync();

       //     return status;
       // }

       // private bool StatusExists(string id)
       // {
       //     return _context.Status.Any(e => e.IdStatus == id);
       // }
    }
}
