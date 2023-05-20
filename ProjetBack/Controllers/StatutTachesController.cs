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
    public class StatutTachesController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public StatutTachesController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/StatutTaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatutTache>>> GetStatutTaches()
        {
            return await _context.StatutTaches.ToListAsync();
        }

        // GET: api/StatutTaches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatutTache>> GetStatutTache(string id)
        {
            var statutTache = await _context.StatutTaches.FindAsync(id);

            if (statutTache == null)
            {
                return NotFound();
            }

            return statutTache;
        }

        // PUT: api/StatutTaches/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutStatutTache(string id, StatutTache statutTache)
        {
            if (id != statutTache.IdStatutTache)
            {
                return BadRequest();
            }

            _context.Entry(statutTache).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatutTacheExists(id))
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

        // POST: api/StatutTaches
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StatutTache>> PostStatutTache(StatutTache statutTache)
        {
            _context.StatutTaches.Add(statutTache);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatutTache", new { id = statutTache.IdStatutTache }, statutTache);
        }

        // DELETE: api/StatutTaches/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<StatutTache>> DeleteStatutTache(string id)
        {
            var statutTache = await _context.StatutTaches.FindAsync(id);
            if (statutTache == null)
            {
                return NotFound();
            }

            _context.StatutTaches.Remove(statutTache);
            await _context.SaveChangesAsync();

            return statutTache;
        }
        [NonAction]
        private bool StatutTacheExists(string id)
        {
            return _context.StatutTaches.Any(e => e.IdStatutTache == id);
        }
    }
}
