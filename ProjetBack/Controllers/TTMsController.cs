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
    public class TTMsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public TTMsController(PilotageDBContext context)
        {
            _context = context;
        }

       // // GET: api/TTMs
       // [HttpGet]
       // public async Task<ActionResult<IEnumerable<TTM>>> GetTTM()
       // {
       //     return await _context.TTM.ToListAsync();
       // }

       // // GET: api/TTMs/5
       // [HttpGet("{id}")]
       // public async Task<ActionResult<TTM>> GetTTM(string id)
       // {
       //     var tTM = await _context.TTM.FindAsync(id);

       //     if (tTM == null)
       //     {
       //         return NotFound();
       //     }

       //     return tTM;
       // }

       // // PUT: api/TTMs/5
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost("Update/{id}")]
       // public async Task<IActionResult> PutTTM(string id, TTM tTM)
       // {
       //     if (id != tTM.IdTTM)
       //     {
       //         return BadRequest();
       //     }

       //     _context.Entry(tTM).State = EntityState.Modified;

       //     try
       //     {
       //         await _context.SaveChangesAsync();
       //     }
       //     catch (DbUpdateConcurrencyException)
       //     {
       //         if (!TTMExists(id))
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

       // // POST: api/TTMs
       // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // // more details see https://aka.ms/RazorPagesCRUD.
       // [HttpPost]
       // public async Task<ActionResult<TTM>> PostTTM(TTM tTM)
       // {
       //     _context.TTM.Add(tTM);
       //     await _context.SaveChangesAsync();

       //     return CreatedAtAction("GetTTM", new { id = tTM.IdTTM }, tTM);
       // }

       // // DELETE: api/TTMs/5
       //[HttpPost("Delete/{id}")]
       // public async Task<ActionResult<TTM>> DeleteTTM(string id)
       // {
       //     var tTM = await _context.TTM.FindAsync(id);
       //     if (tTM == null)
       //     {
       //         return NotFound();
       //     }

       //     _context.TTM.Remove(tTM);
       //     await _context.SaveChangesAsync();

       //     return tTM;
       // }

       // private bool TTMExists(string id)
       // {
       //     return _context.TTM.Any(e => e.IdTTM == id);
       // }
    }
}
