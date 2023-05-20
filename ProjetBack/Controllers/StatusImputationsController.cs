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
    public class StatusImputationsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public StatusImputationsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/StatusImputations
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<StatusImputation>>> GetStatusImputations()
        //{
        //    return await _context.StatusImputations.ToListAsync();
        //}

        // GET: api/StatusImputations/5
        ////[HttpGet("{id}")]
        ////public async Task<ActionResult<StatusImputation>> GetStatusImputation(string id)
        ////{
        ////    var statusImputation = await _context.StatusImputations.FindAsync(id);

        ////    if (statusImputation == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return statusImputation;
        ////}

        //// PUT: api/StatusImputations/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("Update/{id}")]
        //public async Task<IActionResult> PutStatusImputation(string id, StatusImputation statusImputation)
        //{
        //    if (id != statusImputation.IdStatusImputations)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(statusImputation).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StatusImputationExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/StatusImputations
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<StatusImputation>> PostStatusImputation(StatusImputation statusImputation)
        //{
        //    _context.StatusImputations.Add(statusImputation);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetStatusImputation", new { id = statusImputation.IdStatusImputations }, statusImputation);
        //}

        //// DELETE: api/StatusImputations/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<StatusImputation>> DeleteStatusImputation(string id)
        //{
        //    var statusImputation = await _context.StatusImputations.FindAsync(id);
        //    if (statusImputation == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.StatusImputations.Remove(statusImputation);
        //    await _context.SaveChangesAsync();

        //    return statusImputation;
        //}

        //private bool StatusImputationExists(string id)
        //{
        //    return _context.StatusImputations.Any(e => e.IdStatusImputations == id);
        //}
    }
}
