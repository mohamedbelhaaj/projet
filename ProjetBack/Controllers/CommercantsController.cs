using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using ProjetBack.Models;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommercantsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public CommercantsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/Commercants
        [HttpGet]
        public async Task<ActionResult> GetCommercants()
        {

            var result= await _context.Commercants.Select(x => new
            {
                x.id,
                x.FullName ,
                x.Nom,
                x.Prenom,
                x.DateCreation,
                x.Createur,
                nbProjet = x.ProjetEdp.Count

            }).ToListAsync();
            return Ok(result);
        }

        // GET: api/Commercants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commercant>> GetCommercant(string id)
        {
            var commercant = await _context.Commercants.FindAsync(id);

            if (commercant == null)
            {
                return NotFound();
            }

            return commercant;
        }

        // PUT: api/Commercants/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutCommercant(string id, Commercant commercant)
        {
            if (id != commercant.id)
            {
                return BadRequest();
            }

            _context.Entry(commercant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommercantExists(id))
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

        // POST: api/Commercants
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Commercant>> PostCommercant(Commercant commercant)
        {
            _context.Commercants.Add(commercant);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CommercantExists(commercant.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCommercant", new { id = commercant.id }, commercant);
        }

        // DELETE: api/Commercants/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Commercant>> DeleteCommercant(string id)
        {
            var commercant = await _context.Commercants.FindAsync(id);
            if (commercant == null)
            {
                return NotFound();
            }

            _context.Commercants.Remove(commercant);
            await _context.SaveChangesAsync();

            return commercant;
        }
        [NonAction]
        private bool CommercantExists(string id)
        {
            return _context.Commercants.Any(e => e.id == id);
        }
    }
}
