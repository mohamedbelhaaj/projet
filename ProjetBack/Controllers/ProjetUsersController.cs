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
    public class ProjetUsersController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public ProjetUsersController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/ProjetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetUser>>> GetProjetUsers()
        {
            return await _context.ProjetUsers.ToListAsync();
        }

        // GET: api/ProjetUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetUser>> GetProjetUser(string id)
        {
            var projetUser = await _context.ProjetUsers.FindAsync(id);

            if (projetUser == null)
            {
                return NotFound();
            }

            return projetUser;
        }

        // PUT: api/ProjetUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutProjetUser(string id, ProjetUser projetUser)
        {
            if (id != projetUser.ProjetId)
            {
                return BadRequest();
            }

            _context.Entry(projetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjetUserExists(id))
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

        // POST: api/ProjetUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProjetUser>> PostProjetUser(ProjetUser projetUser)
        {
            _context.ProjetUsers.Add(projetUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjetUserExists(projetUser.ProjetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjetUser", new { id = projetUser.ProjetId }, projetUser);
        }

        // DELETE: api/ProjetUsers/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<ProjetUser>> DeleteProjetUser(string id)
        {
            var projetUser = await _context.ProjetUsers.FindAsync(id);
            if (projetUser == null)
            {
                return NotFound();
            }

            _context.ProjetUsers.Remove(projetUser);
            await _context.SaveChangesAsync();

            return projetUser;
        }
        [NonAction]
        private bool ProjetUserExists(string id)
        {
            return _context.ProjetUsers.Any(e => e.ProjetId == id);
        }
    }
}
