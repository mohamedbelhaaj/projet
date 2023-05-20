using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using ProjetBack.Models;
using Microsoft.AspNetCore.Cors;
using ProjetBack.Dtos;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class TachesController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public TachesController(PilotageDBContext context)
        {
            _context = context;
        }
        [HttpGet("GetByIdUser/{iduser}")]
        public ActionResult GetByIdUser(string idUser)
        {


            var result = _context.Taches.Where(x => (x.UserId == idUser || x.publique==true) && x.status!="3").ToList();


            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("GetAllByIdUser/{iduser}")]
        public ActionResult GetAllByIdUser(string idUser)
        {


            var result = _context.Taches.Where(x => x.UserId == idUser ).ToList();


            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Taches
        [HttpGet]
        public async Task<ActionResult> GetTaches()
        {
            // _context.Taches.Include(x => x.User).ToList();
            var resultat1 = (from taches in _context.Taches
                            // join Projects in _context.Projets on taches.ProjetId equals Projects.Id
                             join users in _context.Users on taches.UserId equals users.IdUser
                             select new
                             {

                                 Id = taches.Id,
                                 Description = taches.Description,
                                 Charge = taches.Charge,
                                 Createur = taches.Createur,
                                 UserId = taches.UserId,
                                 StatutTacheId = taches.StatutTacheId,
                               //  ProjetId = taches.ProjetId,
                                 User = users.FullName,
                                //Projet = Projects.Nommenclature,
                                 countTacheInImp = taches.DetailImputations.Count()
                             }).ToList();

            return Ok(resultat1);


        }
        [HttpGet("GetTachesByUser/{IdUser}")]
        public async Task<ActionResult> GetTacheByUser(string IdUser)
        {

            var resultat1 = (from taches in _context.Taches
                            // join Projects in _context.Projets on taches.ProjetId equals Projects.Id
                             //join Projectusers in _context.ProjetUsers on Projects.Id equals Projectusers.ProjetId
                           //  join users in _context.Users on taches.UserId equals users.IdUser where Projectusers.UserId == IdUser
                             select new
                             {

                                 Id = taches.Id,
                                 Description = taches.Description,
                                 Charge = taches.Charge,
                                 Createur = taches.Createur,
                                 UserId = taches.UserId,
                                 StatutTacheId = taches.StatutTacheId,
                               //  ProjetId = taches.ProjetId,
                               //User = users.FullName,
                                // Projet = Projects.Nommenclature,
                                 countTacheInImp = taches.DetailImputations.Count()
                             }).ToList();

            return Ok(resultat1);


        }
        [HttpGet("GetTachesByManagers/{IdManagers}")]

        public async Task<ActionResult> GetTachesByManagers(string IdManagers)
        {
            var resultat1 = (from taches in _context.Taches
                           //  join Projects in _context.Projets on taches.ProjetId equals Projects.Id
                          //   join Projectusers in _context.ProjetUsers on Projects.Id equals Projectusers.ProjetId
                             join users in _context.Users on taches.UserId equals users.IdUser
                           // / where Projectusers.UserId == IdManagers || users.Valideur2Id == IdManagers || users.Valideur2Id == IdManagers
                             select new
                             {
                                 Id = taches.Id,
                                 Description = taches.Description,
                                 Charge = taches.Charge,
                                 Createur = taches.Createur,
                                 UserId = taches.UserId,
                                 StatutTacheId = taches.StatutTacheId,
                              //   ProjetId = taches.ProjetId,
                                 User = users.FullName,
                               //  Projet = Projects.Nommenclature,
                                 countTacheInImp = taches.DetailImputations.Count()
                             }).Distinct().ToList();

            return Ok(resultat1);


        }

        // GET: api/Taches/5
        [HttpGet("{id}")]
        public IActionResult GetTache(string id)
        {
            var tache = (from t in _context.Taches
                          join d in _context.DetailLivraisons on t.detailLivraisonId equals d.Id
                          join p in _context.Projets on d.ProjetId equals p.Id
                          join pl in _context.ProjetLivraisons on d.ProjetLivraisonId equals pl.Id
                          join u in _context.Users on t.UserId equals u.IdUser
                          where t.Id==id
                          select new
                          {
                              Id = t.Id,
                              premierImputation = t.premierImputation,
                              status = t.status,
                              Description = t.Description,
                              Createur = t.Createur,
                              Charge = t.Charge,
                              chargeConsomme = t.chargeConsomme,
                              DateCreation = t.DateCreation,
                              detailLivraisonId = t.detailLivraisonId,
                             // projetLivraisonId = p.Id,
                            t.lienAzure,
                            t.publique,
                              projetLivraisonId = new
                              {
                                  projetName = pl.ProjetName + "  V" + pl.Delivery,

                                  id =  pl.Id,
                              },
                              userName = u.FullName,
                              userId=t.UserId,
                              projetName = p.Nommenclature + " V" + d.Delivery,
                              ProjetLivraisonName = pl.ProjetName + " V" + pl.Delivery,
                              countValidated = t.DetailImputations.Count()

                          }).FirstOrDefault();

            if (tache == null)
            {
                return NotFound();
            }

            return Ok(tache);
        }



        // GET: api/Taches/projet

        //[HttpGet("projet/{id}")]
        //public async Task<ActionResult<Tache>> GetTacheProjet(string id)
        //{
        //    var result = await _context.Taches.Where(x => x.Projet.Id == id).ToListAsync();
        //    return Ok(result);
        //}



        // GET: api/Taches/user

        [HttpPost("GetTacheUser")]
        public async Task<IActionResult> GetTacheUserAsync(Search Search)
        {
            int offset = (Search.page * Search.size);

            var result = (from t in _context.Taches
                                 where (Search.detailLivraisonId == "0" ||  t.detailLivraisonId == Search.detailLivraisonId) &&
                           
                              (Search.status == "0" || t.status == Search.status) &&
                              (Search.type == "0" || t.publique == bool.Parse(Search.type)) &&
                              (Search.idUser == "" || t.UserId == Search.idUser) 
                               join d in _context.DetailLivraisons on t.detailLivraisonId equals d.Id
                              where (Search.projetLivraisonId == "" || d.ProjetLivraisonId == Search.projetLivraisonId)
                              join p in _context.Projets on d.ProjetId equals p.Id
                             join pl in _context.ProjetLivraisons on d.ProjetLivraisonId equals pl.Id
                             join u in _context.Users on t.UserId equals u.IdUser into joinedT
                             from us in joinedT.DefaultIfEmpty()

                              select new
                              {
                                  t.publique,
                                  Id = t.Id,
                                  premierImputation = t.premierImputation,
                                  status = t.status,
                                  Description = t.Description,
                                  Createur = t.Createur,
                                  Charge = t.Charge,
                                  chargeConsomme = t.chargeConsomme,
                                  DateCreation = t.DateCreation,
                                  userName = us == null ? String.Empty : us.FullName,
                                  projetName = p.Nommenclature +" V"+d.Delivery,
                                  ProjetLivraisonName = pl.ProjetName +" V"+ pl.Delivery,
                                  countTacheInImp = t.DetailImputations.Count(),

                                  counttacheNotvalidated = t.DetailImputations.Where(x=>x.Imputation.StatusImputation == 1).Count()
                              }).ToList();

            var nbTotalResults = result.Count();
            var data = result.OrderByDescending(x => x.DateCreation).Skip(offset).Take(Search.size).ToList();

            return Ok(new { nbTotalResults, data });
        }


        [HttpPost("GetTachesByManager")]
        public async Task<IActionResult> GetTachesByManager(Search Search)
        {


            int offset = (Search.page * Search.size);

            var result = (from t in _context.Taches
                          where (Search.detailLivraisonId == "0" || t.detailLivraisonId == Search.detailLivraisonId) &&

                       (Search.status == "0" || t.status == Search.status) &&
                       (Search.type == "0" || t.publique == bool.Parse(Search.type)) &&
                       (Search.idUser == "" || t.UserId == Search.idUser)
                          join d in _context.DetailLivraisons on t.detailLivraisonId equals d.Id
                          where (Search.projetLivraisonId == "" || d.ProjetLivraisonId == Search.projetLivraisonId)
                          join p in _context.Projets on d.ProjetId equals p.Id
                          join pl in _context.ProjetLivraisons on d.ProjetLivraisonId equals pl.Id
                          join pt in _context.ProjetEquipes on p.Id equals pt.ProjetId
                          where (Search.ManagerId == "" || pt.Equipe.ManagerId == Search.ManagerId || t.UserId == Search.ManagerId)

                          select new
                          {
                              t.publique,
                              Id = t.Id,
                              premierImputation = t.premierImputation,
                              status = t.status,
                              Description = t.Description,
                              Createur = t.Createur,
                              Charge = t.Charge,
                              chargeConsomme = t.chargeConsomme,
                              DateCreation = t.DateCreation,
                              userName = t.User == null ? String.Empty : t.User.FullName,
                              projetName = p.Nommenclature + " V" + d.Delivery,
                              ProjetLivraisonName = pl.ProjetName + " V" + pl.Delivery,
                              countTacheInImp = t.DetailImputations.Count(),

                              counttacheNotvalidated = t.DetailImputations.Where(x => x.Imputation.StatusImputation == 1).Count()
                          }).ToList();

            var nbTotalResults = result.Count();
            var data = result.OrderByDescending(x => x.DateCreation).Skip(offset).Take(Search.size).ToList();

            return Ok(new { nbTotalResults, data });




            //var result = (from t in _context.Taches
            //              join d in _context.DetailLivraisons on t.detailLivraisonId equals d.Id
            //              join p in _context.Projets on d.ProjetId equals p.Id


            //              join pl in _context.ProjetLivraisons on d.ProjetLivraisonId equals pl.Id
            //              join u in _context.Users on t.UserId equals u.IdUser
            //              join pt in _context.ProjetEquipes on p.Id equals pt.ProjetId 
                     
            //              where (pt != null) &&
            //              (Search.projetLivraisonId == "0" || d.ProjetLivraisonId == Search.projetLivraisonId) &&

            //              (Search.detailLivraisonId == "0" || t.detailLivraisonId == Search.detailLivraisonId) &&
            //                  (Search.idUser == "" || t.UserId == Search.idUser) &&
            //               (Search.status == "0" || t.status == Search.status)
            //              &&(Search.ManagerId == "" || pt.Equipe.ManagerId == Search.ManagerId || t.UserId == Search.ManagerId)
            //              select new
            //              {
            //                  Id = t.Id,
            //                  premierImputation = t.premierImputation,
            //                  status = t.status,
            //                  Description = t.Description,
            //                  Createur = t.Createur,
            //                  Charge = t.Charge,
            //                  chargeConsomme = t.chargeConsomme,
            //                  DateCreation = t.DateCreation,
            //                  userName =  u.FullName,
            //                  projetName = p.Nommenclature + " V" + d.Delivery,
            //                  ProjetLivraisonName = pl.ProjetName + " V" + pl.Delivery,
            //                  countTacheInImp = t.DetailImputations.Count(),
            //                  ProjetId = pt.ProjetId,

            //                  counttacheNotvalidated = t.DetailImputations.Where(x => x.Imputation.StatusImputation ==1 ).Count()
            //              }).Distinct().ToList();

            //return Ok(result);
        }
        // GET: api/Taches/user

        [HttpGet("user")]
        public async Task<ActionResult<Tache>> GetTaches(string id)
        {
            var result = await _context.Taches.Where(x => x.User.IdUser == id).ToListAsync();
            return Ok(result);
        }


        // PUT: api/Taches/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("Update/{id}")]
        //public async Task<IActionResult> PutTache(string id, Tache tache)
        //{
        //    if (id != tache.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tache).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TacheExists(id))
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


        // POST: api/Taches
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Tache>> PostTache(Tache tache)
        {
            if (string.IsNullOrEmpty(tache.Id))
            {
                if (tache.UserId == "publique")
                    tache.UserId = null;
                tache.Id = null;
                tache.DateCreation = DateTime.Now.ToString();
                _context.Taches.Add(tache);
                
                //await _context.SaveChangesAsync();
            }
            else
            {
                _context.Entry(tache).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTache", new { id = tache.Id }, tache);
        }

        // DELETE: api/Taches/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Tache>> DeleteTache(string id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache == null)
            {
                return NotFound();
            }

            _context.Taches.Remove(tache);
            await _context.SaveChangesAsync();

            return tache;
        }
        [HttpPost("complete/{id}")]
        public async Task<ActionResult<Tache>> completeTache(string id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache == null)
            {
                return NotFound();
            }
            tache.status = "3";
            _context.Entry(tache).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return tache;
        }

        private bool TacheExists(string id)
        {
            return _context.Taches.Any(e => e.Id == id);
        }
    }
}
