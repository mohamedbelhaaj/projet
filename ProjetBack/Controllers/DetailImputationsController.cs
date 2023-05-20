using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using ProjetBack.PushServices;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DetailImputationsController : ControllerBase
    {
        private readonly PilotageDBContext _context;
        private readonly IHubContext<NotificationHub, INotificationHubService> _hubContext;

        public DetailImputationsController(PilotageDBContext context, IHubContext<NotificationHub, INotificationHubService> hubContext)
        {
            _context = context;
            this._hubContext = hubContext;
        }

        // GET: api/DetailImputations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailImputation>>> GetDetailImputations()
        {
            return await _context.DetailImputations.ToListAsync();
        }

        // GET: api/DetailImputations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailImputation>> GetDetailImputation(string id)
        {
            var detailImputation = await _context.DetailImputations.FindAsync(id);

            if (detailImputation == null)
            {
                return NotFound();
            }

            return detailImputation;
        }

        // PUT: api/DetailImputations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutDetailImputation(string id, DetailImputation detailImputation)
        {
            if (id != detailImputation.Id)
            {
                return BadRequest();
            }

            _context.Entry(detailImputation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailImputationExists(id))
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

        // POST: api/DetailImputations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{dateDebut}/{dateFin}")]
        public async Task<ActionResult<DetailImputation>> PostDetailImputation(DateTime dateDebut, DateTime dateFin, DetailImputation detailImputation, string id)
        {

            if (string.IsNullOrEmpty(detailImputation.ImputationId))
            {

                Imputation imputation = new Imputation {
                    DateDebut = dateDebut,
                    DateFin = dateFin,
                
                   UserId = id,
           //     StatusImputationId = _context.StatusImputations.FirstOrDefault(x => x.IsDefault)?.IdStatusImputations,
                    DetailImputations =new List<DetailImputation>
                    {
                        detailImputation
                    }
                };
                _context.Imputations.Add(imputation);
            }
            else
            {
            _context.DetailImputations.Add(detailImputation);
            }
            await _context.SaveChangesAsync();
           
            
            await _hubContext.Clients.All.GetNotifications(new Models.Notification { Title = "add timesheet item", Content="time sheet .... any content", ActionLink="/timesheet/"+ detailImputation.ImputationId });
            

            return CreatedAtAction("GetDetailImputation", new { id = detailImputation.Id }, detailImputation);
        }

        // DELETE: api/DetailImputations/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<DetailImputation>> DeleteDetailImputation(string id)
        {
            var detailImputation = await _context.DetailImputations.FindAsync(id);
            if (detailImputation == null)
            {
                return NotFound();
            }

            _context.DetailImputations.Remove(detailImputation);
            await _context.SaveChangesAsync();

            return detailImputation;
        }
        [NonAction]
        private bool DetailImputationExists(string id)
        {
            return _context.DetailImputations.Any(e => e.Id == id);
        }
    }
}
