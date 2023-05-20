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
    public class DetailLivraisonsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public DetailLivraisonsController(PilotageDBContext context)
        {
            _context = context;
        }

        // GET: api/DetailLivraisons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailLivraison>>> GetDetailLivraisons()
        {
            return await _context.DetailLivraisons.ToListAsync();
        }
        [HttpGet("GetDetailsForAdminForTache")]
        public IActionResult GetDetailsForAdminForTache()
        {

            var resulat = (from detailLivraisons in _context.DetailLivraisons
             
        
                           select new
                           {
                               Id = detailLivraisons.Id,
                               nommenclatureProject = detailLivraisons.Projet.Nommenclature + detailLivraisons.Delivery,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,
                        
                           }).ToList();
            return Ok(resulat);


        }

        [HttpGet("GetDetailsbyProjetLivraisnId/{ProjetLivraisonId}")]
        public IActionResult GetDetailsbyProjetLivraisnId(string ProjetLivraisonId)
        {

            var resulat = (from detailLivraisons in _context.DetailLivraisons

                           where detailLivraisons.ProjetLivraisonId== ProjetLivraisonId
                           select new
                           {
                               Id = detailLivraisons.Id,
                               nommenclatureProject = detailLivraisons.Projet.Nommenclature + detailLivraisons.Delivery,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,

                           }).ToList();
            return Ok(resulat);


        }



        [HttpGet("satatDashboardLivraisonComtable/{idProjetComptable}")]
        public IActionResult satatDashboardLivraisonComtable(string idprojetLivraison)
        {
            try
            {


                var dbresult = (

         

                   
                                from  dl in _context.DetailLivraisons where dl.ProjetLivraisonId== idprojetLivraison

                                join t in _context.Taches on dl.Id equals t.detailLivraisonId
                                             into joinedt
                                from tt in joinedt.DefaultIfEmpty()


                                join pl in _context.ProjetLivraisons on dl.ProjetLivraisonId equals pl.Id

                                join p in _context.profileUser on tt.UserId equals p.userId
                                   into joinedtg
                                from tte in joinedtg.DefaultIfEmpty()

                                join di in _context.DetailImputations on tt.Id equals di.TacheId

                                  into joinedtge
                                from tted in joinedtge.DefaultIfEmpty()



                                select new
                                {

                                    dl.Id,
                                    projectName = dl.Projet.Nommenclature + " " + dl.Delivery,
                                    dl.StatusId,
                                    dl.StartDate,
                                    dl.PlannedDate,
                                    dl.DeliveryDate,


                                    budgetConfirmeTotale = (pl.projetEdp.budgetConfirmeRallonge != null ? pl.projetEdp.budgetConfirmeRallonge : 0) + (pl.projetEdp.budgetConfirme != null ? pl.projetEdp.budgetConfirme : 0),

                                    budgetGPTotale = (pl.projetEdp.budgetGPRallonge != null ? pl.projetEdp.budgetGPRallonge : 0) + (pl.projetEdp.budgetGP != null ? pl.projetEdp.budgetGP : 0),


                                    budgetJuniorTotale = (pl.projetEdp.budgetJunior != null ? pl.projetEdp.budgetJunior : 0) + (pl.projetEdp.budgetJuniorRallonge != null ? pl.projetEdp.budgetJuniorRallonge : 0),

                                    budgetValidationTotale = (pl.projetEdp.budgetValidation != null ? pl.projetEdp.budgetValidation : 0) + (pl.projetEdp.budgetValidationRallonge != null ? pl.projetEdp.budgetValidationRallonge : 0),

                                    budgetSeniorTotale = (pl.projetEdp.budgetSenior != null ? pl.projetEdp.budgetSenior : 0) + (pl.projetEdp.budgetSeniorRallonge != null ? pl.projetEdp.budgetSeniorRallonge : 0),

                                    budgetDirectionTotale = (pl.projetEdp.budgetDirection != null ? pl.projetEdp.budgetDirection : 0) + (pl.projetEdp.budgetDirectionRallonge != null ? pl.projetEdp.budgetDirectionRallonge : 0),



                                    tte.profileId,
                                    Monday = (Convert.ToDouble(tted.Monday.Substring(0, 2)) + Convert.ToDouble(tted.Monday.Substring(3, 2)) / 60) / 8,
                                    Tuesday = (Convert.ToDouble(tted.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted.Tuesday.Substring(3, 2)) / 60) / 8,
                                    Friday = (Convert.ToDouble(tted.Friday.Substring(0, 2)) + Convert.ToDouble(tted.Friday.Substring(3, 2)) / 60) / 8,
                                    Wednesday = (Convert.ToDouble(tted.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted.Wednesday.Substring(3, 2)) / 60) / 8,
                                    Thursday = (Convert.ToDouble(tted.Thursday.Substring(0, 2)) + Convert.ToDouble(tted.Thursday.Substring(3, 2)) / 60) / 8,


                                }).ToList();




                var result1 = (from edp in dbresult
                               group edp by new
                               {
                                   edp.profileId,

                                   edp.Id,
                                   edp.projectName,
                                   edp.StatusId,
                                   edp.StartDate,
                                   edp.PlannedDate,
                                   edp.DeliveryDate,

                                   edp.budgetDirectionTotale,
                                   edp.budgetConfirmeTotale,
                                   edp.budgetGPTotale,
                                   edp.budgetJuniorTotale,
                                   edp.budgetValidationTotale,
                                   edp.budgetSeniorTotale,





                               } into g
                               select new
                               {


                                   g.Key.profileId,

                                   g.Key.projectName,
                                   g.Key.Id,
                                   g.Key.StatusId,
                                   g.Key.StartDate,
                                   g.Key.PlannedDate,
                                   g.Key.DeliveryDate,


                                   coutEstime = (g.Key.profileId == "confirmé" ? g.Key.budgetConfirmeTotale
                                    : (g.Key.profileId == "Direction" ? g.Key.budgetDirectionTotale
                                    : (g.Key.profileId == "GP" ? g.Key.budgetGPTotale
                                    : (g.Key.profileId == "junior" ? g.Key.budgetJuniorTotale
                                    : (g.Key.profileId == "sénior" ? g.Key.budgetSeniorTotale
                            : (g.Key.profileId == "Validation" ? g.Key.budgetValidationTotale
                   : 0)))))
                    ) * Convert.ToDouble(_context.profile.Where(x => x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault()),


                                   charge = g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday),

                                   cout = (g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday))

                                  * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault())
                               }).ToList();



                var ResultFinal = (from edp in result1
                                   group edp by new
                                   {

                                       edp.Id,
                                       edp.projectName,
                                       edp.StatusId,
                                       edp.StartDate,
                                       edp.PlannedDate,
                                       edp.DeliveryDate,
                 
                                   } into g
                                   select new
                                   {
                                       g.Key.projectName,
                                       g.Key.Id,
                                       g.Key.StatusId,
                                       g.Key.StartDate,
                                       g.Key.PlannedDate,
                                       g.Key.DeliveryDate,
                                       showDetails = false,
                                      coutEstime = g.Sum(x => x.coutEstime),
                                       charge = g.Sum(x => x.charge),
                                       cout = g.Sum(x => x.cout)
                                   }).ToList();

                return Ok(ResultFinal);

            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }







        [HttpGet("GetProjetDetailLivraisonForTasks/{idUser}")]
        public IActionResult GetProjetDetailLivraisonForTasks(string idUser)
        {

            var resulat = (

                           from  detailLivraisons in _context.DetailLivraisons
                           join  taches in _context.Taches on detailLivraisons.Id equals taches.detailLivraisonId
                    
                           where (taches.UserId == idUser || detailLivraisons.Projet.Publique == "true" ) && taches.status != "3"

                          // group new { detailLivraisons } by new { detailLivraisons.Projet.Nommenclature, detailLivraisons.PlannedDate, detailLivraisons.Id, detailLivraisons.Delivery, detailLivraisons.StartDate, detailLivraisons.DeliveryDate, detailLivraisons.ProjetLivraisonId } into grp
            select new
                           {

                               NommenclatureProject = detailLivraisons.Projet.Nommenclature + "V" + detailLivraisons.Delivery,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,
                               Id = detailLivraisons.Id,
                               DeliveryDate = detailLivraisons.DeliveryDate,
                               StartDate = detailLivraisons.StartDate,
                               detailLivraisons.PlannedDate


                           }).Distinct().ToList();
            return Ok(resulat);


        }
        [HttpGet("GetDetailsByUser/{idUser}")]
        public IActionResult GetDetailsByUser(string idUser)
        {

            var resulat = (from detailLivraisons in _context.DetailLivraisons
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where userEquipe.UserId == idUser 
                           // group new { detailLivraisons , project} by new { detailLivraisons.Id, detailLivraisons.ProjetLivraisonId, project.Nommenclature } into grp
                           select new
                           {
                               Id = detailLivraisons.Id,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,
                               NommenclatureProject = project.Nommenclature +" V"+detailLivraisons.Delivery,
                               projectEquipe = project.projetsEquipe


                           }).Distinct().ToList();

            return Ok(resulat);


        }




        [HttpGet("GetDetailsByEquipe/{idEquipe}")]
        public IActionResult GetDetailsByEquipe(string idEquipe)
        {

            var resulat = (from  detailLivraisons in _context.DetailLivraisons
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           where ProjetEquipes.EquipeId == idEquipe 
                          // group new { detailLivraisons , project} by new { detailLivraisons.Id, detailLivraisons.ProjetLivraisonId, project.Nommenclature } into grp
                           select new
                           {
                               Id=  detailLivraisons.Id,
                               ProjetLivraisonId =  detailLivraisons.ProjetLivraisonId ,
                               NommenclatureProject= project.Nommenclature + " V" + detailLivraisons.Delivery,
                               projectEquipe=project.projetsEquipe
                        

                           }).ToList();
            return Ok(resulat);


        }

        [HttpGet("GetDetailsByManager/{idManager}")]
        public IActionResult GetDetailsByManager(string idManager)
        {
          

            var resulat = (from detailLivraisons in _context.DetailLivraisons
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join equipe in _context.Equips on ProjetEquipes.EquipeId equals equipe.Id
                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where (userEquipe.UserId == idManager || equipe.ManagerId == idManager) 
                           // group new { detailLivraisons , project} by new { detailLivraisons.Id, detailLivraisons.ProjetLivraisonId, project.Nommenclature } into grp
                           select new
                           {
                               Id = detailLivraisons.Id,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,
                               NommenclatureProject = project.Nommenclature + " V" + detailLivraisons.Delivery,
                               projectEquipe = project.projetsEquipe


                           }).Distinct().ToList();
            return Ok(resulat);


        }




        [HttpGet("GetDetailsByManager2WeeksBefore/{idManager}")]
        public IActionResult GetDetailsByManager2WeeksBefore(string idManager)
        {
            var date = DateTime.Now.AddDays(-21);

            var resulat = (from detailLivraisons in _context.DetailLivraisons
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join equipe in _context.Equips on ProjetEquipes.EquipeId equals equipe.Id
                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where (userEquipe.UserId == idManager || equipe.ManagerId == idManager) && detailLivraisons.DeliveryDate >= date.Date

                           // group new { detailLivraisons , project} by new { detailLivraisons.Id, detailLivraisons.ProjetLivraisonId, project.Nommenclature } into grp
                           select new
                           {
                               Id = detailLivraisons.Id,
                               ProjetLivraisonId = detailLivraisons.ProjetLivraisonId,
                               NommenclatureProject = project.Nommenclature + " V" + detailLivraisons.Delivery,
                               projectEquipe = project.projetsEquipe


                           }).Distinct().ToList();
            return Ok(resulat);


        }




        // GET: api/DetailLivraisons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailLivraison>> GetDetailLivraison(string id)
        {
            var detailLivraison = await _context.DetailLivraisons.FindAsync(id);

            if (detailLivraison == null)
            {
                return NotFound();
            }

            return detailLivraison;
        }

        // PUT: api/DetailLivraisons/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutDetailLivraison(string id, DetailLivraison detailLivraison)
        {
            if (id != detailLivraison.Id)
            {
                return BadRequest();
            }

            _context.Entry(detailLivraison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailLivraisonExists(id))
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

        // POST: api/DetailLivraisons
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DetailLivraison>> PostDetailLivraison(DetailLivraison detailLivraison)
        {
            _context.DetailLivraisons.Add(detailLivraison);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetailLivraison", new { id = detailLivraison.Id }, detailLivraison);
        }

        // DELETE: api/DetailLivraisons/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<DetailLivraison>> DeleteDetailLivraison(string id)
        {
            var detailLivraison = await _context.DetailLivraisons.FindAsync(id);
            if (detailLivraison == null)
            {
                return NotFound();
            }

            _context.DetailLivraisons.Remove(detailLivraison);
            await _context.SaveChangesAsync();

            return detailLivraison;
        }
        [NonAction]
        private bool DetailLivraisonExists(string id)
        {
            return _context.DetailLivraisons.Any(e => e.Id == id);
        }
    }
}
