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
    public class PhaseProjetsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public PhaseProjetsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/PhaseProjets
       // [HttpGet]
       // public async Task<ActionResult<IEnumerable<PhaseProjet>>> GetPhaseProjets()
       // {
       //     return await _context.PhaseProjets.ToListAsync();
       // }

       // // GET: api/PhaseProjets/5
       // [HttpGet("{id}")]
       // public async Task<ActionResult<PhaseProjet>> GetPhaseProjet(string id)
       // {
       //     var phaseProjet = await _context.PhaseProjets.FindAsync(id);

       //     if (phaseProjet == null)
       //     {
       //         return NotFound();
       //     }

       //     return phaseProjet;
       // }

       // // PUT: api/PhaseProjets/5
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost("Update/{id}")]
       // public async Task<IActionResult> PutPhaseProjet(string id, PhaseProjet phaseProjet)
       // {
       //     if (id != phaseProjet.Id)
       //     {
       //         return BadRequest();
       //     }

       //     _context.Entry(phaseProjet).State = EntityState.Modified;

       //     try
       //     {
       //         await _context.SaveChangesAsync();
       //     }
       //     catch (DbUpdateConcurrencyException)
       //     {
       //         if (!PhaseProjetExists(id))
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

       // // POST: api/PhaseProjets
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost]
       // public async Task<ActionResult<PhaseProjet>> PostPhaseProjet(PhaseProjet phaseProjet)
       // {
       //     _context.PhaseProjets.Add(phaseProjet);
       //     await _context.SaveChangesAsync();

       //     return CreatedAtAction("GetPhaseProjet", new { id = phaseProjet.Id }, phaseProjet);
       // }

       // // DELETE: api/PhaseProjets/5
       //[HttpPost("Delete/{id}")]
       // public async Task<ActionResult<PhaseProjet>> DeletePhaseProjet(string id)
       // {
       //     var phaseProjet = await _context.PhaseProjets.FindAsync(id);
       //     if (phaseProjet == null)
       //     {
       //         return NotFound();
       //     }

       //     _context.PhaseProjets.Remove(phaseProjet);
       //     await _context.SaveChangesAsync();

       //     return phaseProjet;
       // }

       // private bool PhaseProjetExists(string id)
       // {
       //     return _context.PhaseProjets.Any(e => e.Id == id);
       // }
    }
}
