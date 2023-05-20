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
    public class TypeClientsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public TypeClientsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/TypeClients
       // [HttpGet]
       // public async Task<ActionResult<IEnumerable<TypeClient>>> GetTypeClients()
       // {
       //     return await _context.TypeClients.ToListAsync();
       // }

       // // GET: api/TypeClients/5
       // [HttpGet("{id}")]
       // public async Task<ActionResult<TypeClient>> GetTypeClient(string id)
       // {
       //     var typeClient = await _context.TypeClients.FindAsync(id);

       //     if (typeClient == null)
       //     {
       //         return NotFound();
       //     }

       //     return typeClient;
       // }

       // // PUT: api/TypeClients/5
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost("Update/{id}")]
       // public async Task<IActionResult> PutTypeClient(string id, TypeClient typeClient)
       // {
       //     if (id != typeClient.Id)
       //     {
       //         return BadRequest();
       //     }

       //     _context.Entry(typeClient).State = EntityState.Modified;

       //     try
       //     {
       //         await _context.SaveChangesAsync();
       //     }
       //     catch (DbUpdateConcurrencyException)
       //     {
       //         if (!TypeClientExists(id))
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

       // // POST: api/TypeClients
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost]
       // public async Task<ActionResult<TypeClient>> PostTypeClient(TypeClient typeClient)
       // {
       //     _context.TypeClients.Add(typeClient);
       //     await _context.SaveChangesAsync();

       //     return CreatedAtAction("GetTypeClient", new { id = typeClient.Id }, typeClient);
       // }

       // // DELETE: api/TypeClients/5
       //[HttpPost("Delete/{id}")]
       // public async Task<ActionResult<TypeClient>> DeleteTypeClient(string id)
       // {
       //     var typeClient = await _context.TypeClients.FindAsync(id);
       //     if (typeClient == null)
       //     {
       //         return NotFound();
       //     }

       //     _context.TypeClients.Remove(typeClient);
       //     await _context.SaveChangesAsync();

       //     return typeClient;
       // }

       // private bool TypeClientExists(string id)
       // {
       //     return _context.TypeClients.Any(e => e.Id == id);
       // }
    }
}
