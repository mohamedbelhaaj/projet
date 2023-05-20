using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetBack.Models;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class EquipeController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public EquipeController(PilotageDBContext context)
        {
            _context = context;
        }

        [HttpGet("getEquipeByUser/{userId}")]
        public IActionResult getEquipeByUser(string userId)
        {
            var result = (from equips in _context.Equips

                          join userEquipe in _context.EquipeUser on equips.Id equals userEquipe.EquipeId

                          where userEquipe.UserId== userId
                          select new
                          {
                              Nom = equips.Nom,
                              Id = equips.Id,
                          
                          }).ToList();

            return Ok(result);

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = (from equips in _context.Equips
                  
                          join user in _context.Users on equips.ManagerId equals user.IdUser
                       
                          select new
                          {
                              Nom = equips.Nom,
                              Id = equips.Id,
                              ManagerId = equips.ManagerId,
                              ManagerName = user.FullName,
                              nbEquipe = equips.equipeUsers.Count,
                              nbProjet=equips.projetsEquipe.Count
                          }).ToList();

            return Ok(result);

        }
        //[HttpGet("GetUsersByEquipe/{equipeId}")]
        //public ActionResult<List<User>> GetUsersByEquipe(string equipeId)
        //{
        //    return _context.Users.Where(x => x.EquipeId == equipeId).ToList();

        //}

        [HttpGet("byManager/{idManager}")]
        public ActionResult<List<Equipe>> GetByManager(string idManager)
        {

            var result = (from equips in _context.Equips
                          join user in _context.Users on equips.ManagerId equals user.IdUser
                          where equips.ManagerId == idManager
                          select new
                          {
                              Nom = equips.Nom,
                              Id = equips.Id,
                              ManagerId = equips.ManagerId,
                              ManagerName = user.FullName,
                              nbEquipe = equips.equipeUsers.Count,
                              nbProjet = equips.projetsEquipe.Count
                          }).ToList();

            return Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var result= _context.Equips.Where(x => x.Id == id).Include(x => x.equipeUsers).Select(x => new
            {
                x.Id,
                x.Nom,
                managerName=   x.Manager.FullName,
                managerId=  x.ManagerId,
                x.equipeUsers,
                x.DateCreation,
                x.Createur


            }).FirstOrDefault();


            return Ok(result);

        }

        [HttpPost]
        public async Task<ActionResult> Post(Equipe equipe)
        {
            var equipeUsers = equipe.equipeUsers;
            equipe.equipeUsers = null;
            equipe.DateCreation = DateTime.Now.ToString();
            _context.Equips.Add(equipe);
            foreach (var item in equipeUsers)
            {
                item.EquipeId = equipe.Id;
            }
            _context.EquipeUser.AddRange(equipeUsers);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Clients/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Equipe>> Delete(string id)
        {
            var equipe = await _context.Equips.FindAsync(id);
            if (equipe == null)
            {
                return NotFound();
            }

            _context.Equips.Remove(equipe);
            await _context.SaveChangesAsync();

            return equipe;
        }



        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Put(string id, Equipe equipe)
        {
            if (id != equipe.Id)
            {
                return BadRequest();
            }
            var equipeUsers = await _context.EquipeUser.Where(x => x.EquipeId == equipe.Id).ToListAsync();
            _context.EquipeUser.RemoveRange(equipeUsers);
            _context.EquipeUser.AddRange(equipe.equipeUsers);

            _context.Entry(equipe).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }




    }
}
