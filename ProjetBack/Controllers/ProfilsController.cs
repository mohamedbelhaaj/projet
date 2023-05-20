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
    public class ProfilsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public ProfilsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/Profils
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profil>>> GetProfils()
        {
            return await _context.Profils.ToListAsync();
        }

        // GET: api/Profils/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profil>> GetProfil(string id)
        {
            var profil = await _context.Profils.FindAsync(id);

            if (profil == null)
            {
                return NotFound();
            }

            return profil;
        }

        // PUT: api/Profils/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutProfil(string id, Profil profil)
        {
            if (id != profil.IdProfil)
            {
                return BadRequest();
            }

            _context.Entry(profil).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfilExists(id))
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

        // POST: api/Profils
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Profil>> PostProfil(Profil profil)
        {
            _context.Profils.Add(profil);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfil", new { id = profil.IdProfil }, profil);
        }

        // DELETE: api/Profils/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Profil>> DeleteProfil(string id)
        {
            var profil = await _context.Profils.FindAsync(id);
            if (profil == null)
            {
                return NotFound();
            }

            _context.Profils.Remove(profil);
            await _context.SaveChangesAsync();

            return profil;
        }
        [NonAction]
        private bool ProfilExists(string id)
        {
            return _context.Profils.Any(e => e.IdProfil == id);
        }
    }
}
