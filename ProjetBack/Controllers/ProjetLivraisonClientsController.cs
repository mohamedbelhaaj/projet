using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetLivraisonClientsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public ProjetLivraisonClientsController(PilotageDBContext context)
        {
            _context = context;
        }
        /*
        // GET: api/ProjetLivraisonClients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetLivraisonClient>>> GetProjetLivraisonClients()
        {
            return await _context.ProjetLivraisonClients.ToListAsync();
        }

        // GET: api/ProjetLivraisonClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetLivraisonClient>> GetProjetLivraisonClient(string id)
        {
            var projetLivraisonClient = await _context.ProjetLivraisonClients.FindAsync(id);

            if (projetLivraisonClient == null)
            {
                return NotFound();
            }

            return projetLivraisonClient;
        }

        // PUT: api/ProjetLivraisonClients/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutProjetLivraisonClient(string id, ProjetLivraisonClient projetLivraisonClient)
        {
            if (id != projetLivraisonClient.ProjetLivraisonId)
            {
                return BadRequest();
            }

            _context.Entry(projetLivraisonClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjetLivraisonClientExists(id))
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

        // POST: api/ProjetLivraisonClients
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProjetLivraisonClient>> PostProjetLivraisonClient(ProjetLivraisonClient projetLivraisonClient)
        {
            _context.ProjetLivraisonClients.Add(projetLivraisonClient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjetLivraisonClientExists(projetLivraisonClient.ProjetLivraisonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjetLivraisonClient", new { id = projetLivraisonClient.ProjetLivraisonId }, projetLivraisonClient);
        }

        // DELETE: api/ProjetLivraisonClients/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<ProjetLivraisonClient>> DeleteProjetLivraisonClient(string id)
        {
            var projetLivraisonClient = await _context.ProjetLivraisonClients.FindAsync(id);
            if (projetLivraisonClient == null)
            {
                return NotFound();
            }

            _context.ProjetLivraisonClients.Remove(projetLivraisonClient);
            await _context.SaveChangesAsync();

            return projetLivraisonClient;
        }

        private bool ProjetLivraisonClientExists(string id)
        {
            return _context.ProjetLivraisonClients.Any(e => e.ProjetLivraisonId == id);
        }*/
    }
}
