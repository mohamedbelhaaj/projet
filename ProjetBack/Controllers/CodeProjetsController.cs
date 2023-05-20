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
    public class CodeProjetsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public CodeProjetsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/CodeProjets
        [HttpGet]
        public async Task<IActionResult> GetCodeProjet()
        {

            var resultat=await _context.CodeProjet.Select(x => new
            {
               nomClient= x.Client.Nom,
                x.ClientId,
                x.id,
                x.Nature,
                x.Numero,
                countProjetCom = x.ProjetEdps.Count,
                x.Intitule
            }).ToListAsync();

            return Ok(resultat);
        }

        // GET: api/CodeProjets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeProjet>> GetCodeProjet(string id)
        {
            
            var codeProjet = await _context.CodeProjet.Where(x=>x.id==id).FirstOrDefaultAsync();

            if (codeProjet == null)
            {
                return NotFound();
            }

            return codeProjet;
        }

        // PUT: api/CodeProjets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutCodeProjet(string id, CodeProjet codeProjet)
        {


            if (id != codeProjet.id)
            {
                return BadRequest();
            }
            var codeProj= await _context.CodeProjet.Where(x=>x.Numero == codeProjet.Numero && x.id!= codeProjet.id).FirstOrDefaultAsync();
            if (codeProj!=null)
            {
                return Ok(new { codeRetour = "500" });
            }
            var client = await _context.Clients.FindAsync(codeProjet.ClientId);
            codeProjet.Intitule = codeProjet.Nature + " | " + client.Nom;

            _context.Entry(codeProjet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { codeRetour = "200" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeProjetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/CodeProjets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CodeProjet>> PostCodeProjet(CodeProjet codeProjet)
        {
            var codeProj =await  _context.CodeProjet.Where(x => x.Numero == codeProjet.Numero).FirstOrDefaultAsync();
            if (codeProj != null)
            {
                return Ok(new { codeRetour = "500" });
            }

            codeProjet.dateCreation = DateTime.Now;

            var client = await _context.Clients.FindAsync(codeProjet.ClientId);
            codeProjet.Intitule = codeProjet.Nature + " | " + client.Nom;
            _context.CodeProjet.Add(codeProjet);
            try
            {

                await _context.SaveChangesAsync();
                return Ok(new { codeRetour = "200" });


            }
            catch (DbUpdateException)
            {
                if (CodeProjetExists(codeProjet.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/CodeProjets/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<CodeProjet>> DeleteCodeProjet(string id)
        {

            var codeProjet = await _context.CodeProjet.FindAsync(id);
            if (codeProjet == null)
            {
                return NotFound();
            }

            _context.CodeProjet.Remove(codeProjet);
            await _context.SaveChangesAsync();

            return codeProjet;
        }
        [NonAction]
        private bool CodeProjetExists(string id)
        {
            return _context.CodeProjet.Any(e => e.Numero == id);
        }
    }
}
