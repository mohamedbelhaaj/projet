using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using ProjetBack.Dtos;
using System.Globalization;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    [EnableCors("CorsPolicy")]

    public class ProjetLivraisonsController : ControllerBase
    {
        private readonly PilotageDBContext _context;
        private ProjetLivraison projetLivraison;
        public ProjetLivraisonsController(PilotageDBContext context)
        {
            _context = context;
        }


        [HttpGet("statByProjetCommercant/{idProjet}")]
        public IActionResult StatProjetCompatble(string idProjet)
        {

            var resultat = (from pl in _context.ProjetLivraisons where pl.projetEdpId==idProjet
                            select new
                            {
                                pl.Id,
                                nom = pl.ProjetName,
                                PlannedDate = pl.PlannedDate,
                                InitialPlannedDate = pl.InitialPlannedDate,

                                DeliveryDate = pl.DeliveryDate,
                                Delivery = pl.Delivery,
                                StartDate = pl.StartDate,
                                StatusId = pl.StatusId,
                                TTMId = pl.TTMId,
                                dateCreation = Convert.ToDateTime(pl.DateCreation),
                                chargeConsommerTotale = (
                                                         from tache in _context.Taches
                                                         join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                                                         join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
                                                         where Livraison.Id == pl.Id 
                                                         select
Livraison.DetailLivraisons.SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme))).First(),
                                //edp1.ProjetLivraisons.SelectMany(x => x.DetailLivraisons).SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme))).First(),
                                countProjetLivraison = pl.DetailLivraisons.Count()
                            }).Distinct().ToList();



            return Ok(resultat);
        }





        [HttpPost("satatDashboardLivraisonComtableDetails")]
        public IActionResult satatDashboardLivraisonComtable(ProjetLivraisonInDto model)
        {
            try
            {
                var date = Convert.ToDateTime(model.dateFin);

                //Changes here 
                if (model.dateDebut.Equals("1000-01-01 00:00:00") && model.dateFin.Equals("9999-01-01 23:59:59"))
                {
                    var dbresult = (

                                                 from pl in _context.ProjetLivraisons.AsEnumerable()


                                                 where pl.projetEdpId == model.id
                                                 join dl in _context.DetailLivraisons on pl?.Id equals dl.ProjetLivraisonId
                                                    into joinedd
                                //
                                from dd in joinedd.DefaultIfEmpty()

                                                 join t in _context.Taches on dd?.Id equals t.detailLivraisonId
                                                              into joinedt
                                                 from tt in joinedt.DefaultIfEmpty()
                                                 join p in _context.profileUser on tt?.UserId equals p.userId
                                                    into joinedtg
                                                 from tte in joinedtg.DefaultIfEmpty()

                                                 join di in _context.DetailImputations on tt?.Id equals di?.TacheId

                                                   into joinedtge
                                                 from tted in joinedtge.DefaultIfEmpty()

                                          
                                                 select new
                                                 {

                                                     pl.Id,
                                                     projectName = pl.ProjetName + " " + pl.Delivery,
                                                     pl.StatusId,
                                                     pl.StartDate,
                                                     pl.PlannedDate,
                                                     pl.DeliveryDate,
                                                     pl.Nature,
                                                     pl.scope,
                                                     client = pl?.Client?.Nom,


                                    //budgetConfirmeTotale = (pl.projetEdp.budgetConfirmeRallonge != null ? pl.projetEdp.budgetConfirmeRallonge : 0) + (pl.projetEdp.budgetConfirme != null ? pl.projetEdp.budgetConfirme : 0),

                                    //budgetGPTotale = (pl.projetEdp.budgetGPRallonge != null ? pl.projetEdp.budgetGPRallonge : 0) + (pl.projetEdp.budgetGP != null ? pl.projetEdp.budgetGP : 0),


                                    //budgetJuniorTotale = (pl.projetEdp.budgetJunior != null ? pl.projetEdp.budgetJunior : 0) + (pl.projetEdp.budgetJuniorRallonge != null ? pl.projetEdp.budgetJuniorRallonge : 0),

                                    //budgetValidationTotale = (pl.projetEdp.budgetValidation != null ? pl.projetEdp.budgetValidation : 0) + (pl.projetEdp.budgetValidationRallonge != null ? pl.projetEdp.budgetValidationRallonge : 0),

                                    //budgetSeniorTotale = (pl.projetEdp.budgetSenior != null ? pl.projetEdp.budgetSenior : 0) + (pl.projetEdp.budgetSeniorRallonge != null ? pl.projetEdp.budgetSeniorRallonge : 0),

                                    //budgetDirectionTotale = (pl.projetEdp.budgetDirection != null ? pl.projetEdp.budgetDirection : 0) + (pl.projetEdp.budgetDirectionRallonge != null ? pl.projetEdp.budgetDirectionRallonge : 0),



                                    tte?.profileId,
                                                     Monday = (Convert.ToDouble(tted?.Monday.Substring(0, 2)) + Convert.ToDouble(tted?.Monday.Substring(3, 2)) / 60) / 8,
                                                     Tuesday = (Convert.ToDouble(tted?.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted?.Tuesday.Substring(3, 2)) / 60) / 8,
                                                     Friday = (Convert.ToDouble(tted?.Friday.Substring(0, 2)) + Convert.ToDouble(tted?.Friday.Substring(3, 2)) / 60) / 8,
                                                     Wednesday = (Convert.ToDouble(tted?.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted?.Wednesday.Substring(3, 2)) / 60) / 8,
                                                     Thursday = (Convert.ToDouble(tted?.Thursday.Substring(0, 2)) + Convert.ToDouble(tted?.Thursday.Substring(3, 2)) / 60) / 8,


                                                 }).ToList();




                    var result1 = (from edp in dbresult
                                   group edp by new
                                   {
                                       edp.profileId,

                                       edp.scope,
                                       edp.Id,
                                       edp.projectName,
                                       edp.StatusId,
                                       edp.StartDate,
                                       edp.PlannedDate,
                                       edp.DeliveryDate,
                                       edp?.client,
                                       edp.Nature,

                                       //edp.budgetDirectionTotale,
                                       //edp.budgetConfirmeTotale,
                                       //edp.budgetGPTotale,
                                       //edp.budgetJuniorTotale,
                                       //edp.budgetValidationTotale,
                                       //edp.budgetSeniorTotale,





                                   } into g
                                   select new
                                   {


                                       g.Key.profileId,
                                       g.Key.Nature,

                                       g.Key.projectName,
                                       g.Key.Id,
                                       g.Key.StatusId,
                                       g.Key.StartDate,
                                       g.Key.PlannedDate,
                                       g.Key.DeliveryDate,
                                       g.Key.client,
                                       g.Key.scope,


                                       //                coutEstime = (g.Key.profileId == "confirmé" ? g.Key.budgetConfirmeTotale
                                       //                 : (g.Key.profileId == "Direction" ? g.Key.budgetDirectionTotale
                                       //                 : (g.Key.profileId == "GP" ? g.Key.budgetGPTotale
                                       //                 : (g.Key.profileId == "junior" ? g.Key.budgetJuniorTotale
                                       //                 : (g.Key.profileId == "sénior" ? g.Key.budgetSeniorTotale
                                       //         : (g.Key.profileId == "Validation" ? g.Key.budgetValidationTotale
                                       //: 0)))))
                                       // ) * Convert.ToDouble(_context.profile.Where(x => x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault()),


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
                                           edp.client,
                                           edp.scope,
                                           edp.Nature,

                                       } into g
                                       select new
                                       {
                                           g.Key.Nature,
                                           g.Key.projectName,
                                           g.Key.Id,
                                           g.Key.StatusId,
                                           g.Key.StartDate,
                                           g.Key.PlannedDate,
                                           g.Key.DeliveryDate,
                                           g.Key.client,
                                           g.Key.scope,
                                           showDetails = false,
                                           coutEstime = 0,
                                           charge = g.Sum(x => x.charge),
                                           cout = g.Sum(x => x.cout)
                                       }).ToList();




                    return Ok(ResultFinal);
                }
                  
                
                else
                {
                    var dbresult = (

                                  from pl in _context.ProjetLivraisons.AsEnumerable()


                                  where pl.projetEdpId == model.id
                                  join dl in _context.DetailLivraisons on pl?.Id equals dl.ProjetLivraisonId
                                     into joinedd
                                //
                                from dd in joinedd.DefaultIfEmpty()

                                  join t in _context.Taches on dd?.Id equals t.detailLivraisonId
                                               into joinedt
                                  from tt in joinedt.DefaultIfEmpty()
                                  join p in _context.profileUser on tt?.UserId equals p.userId
                                     into joinedtg
                                  from tte in joinedtg.DefaultIfEmpty()

                                  join di in _context.DetailImputations on tt?.Id equals di.TacheId

                                    into joinedtge
                                  from tted in joinedtge.DefaultIfEmpty()

                                  join imputa in _context.Imputations on tted?.ImputationId equals imputa.Id
                                   into NewFiltre
                                  from rc in NewFiltre
                                  where ((rc.mondayDate.Value.Date < Convert.ToDateTime(model.dateFin) && rc.mondayDate.Value.Date > Convert.ToDateTime(model.dateDebut)

                                         && rc.DetailImputations.Select(e => e.Monday).Any(e => e != "00:00"))
                                         ||
                                         (rc.TuesdayDate.Value.Date < Convert.ToDateTime(model.dateFin) && rc.TuesdayDate.Value.Date > Convert.ToDateTime(model.dateDebut)

                                         && rc.DetailImputations.Select(e => e.Tuesday).Any(e => e != "00:00"))
                                         ||
                                         (rc.WednesdayDate.Value.Date < Convert.ToDateTime(model.dateFin) && rc.WednesdayDate.Value.Date > Convert.ToDateTime(model.dateDebut)

                                         && rc.DetailImputations.Select(e => e.Wednesday).Any(e => e != "00:00"))
                                         ||
                                         (rc.ThursdayDate.Value.Date < Convert.ToDateTime(model.dateFin) && rc.ThursdayDate.Value.Date > Convert.ToDateTime(model.dateDebut)

                                         && rc.DetailImputations.Select(e => e.Thursday).Any(e => e != "00:00"))
                                         ||
                                         (rc.FridayDate.Value.Date < Convert.ToDateTime(model.dateFin) && rc.FridayDate.Value.Date > Convert.ToDateTime(model.dateDebut)

                                         && rc.DetailImputations.Select(e => e.Friday).Any(e => e != "00:00"))


                                             )

                                  where (pl.PlannedDate.Date <= Convert.ToDateTime(model.dateFin) || pl.PlannedDate.Date >= Convert.ToDateTime(model.dateDebut))
                                  select new
                                  {

                                      pl.Id,
                                      projectName = pl.ProjetName + " " + pl.Delivery,
                                      pl.StatusId,
                                      pl.StartDate,
                                      pl.PlannedDate,
                                      pl.DeliveryDate,
                                      pl.Nature,
                                      pl.scope,
                                      client = pl?.Client?.Nom,


                                      //budgetConfirmeTotale = (pl.projetEdp.budgetConfirmeRallonge != null ? pl.projetEdp.budgetConfirmeRallonge : 0) + (pl.projetEdp.budgetConfirme != null ? pl.projetEdp.budgetConfirme : 0),

                                      //budgetGPTotale = (pl.projetEdp.budgetGPRallonge != null ? pl.projetEdp.budgetGPRallonge : 0) + (pl.projetEdp.budgetGP != null ? pl.projetEdp.budgetGP : 0),


                                      //budgetJuniorTotale = (pl.projetEdp.budgetJunior != null ? pl.projetEdp.budgetJunior : 0) + (pl.projetEdp.budgetJuniorRallonge != null ? pl.projetEdp.budgetJuniorRallonge : 0),

                                      //budgetValidationTotale = (pl.projetEdp.budgetValidation != null ? pl.projetEdp.budgetValidation : 0) + (pl.projetEdp.budgetValidationRallonge != null ? pl.projetEdp.budgetValidationRallonge : 0),

                                      //budgetSeniorTotale = (pl.projetEdp.budgetSenior != null ? pl.projetEdp.budgetSenior : 0) + (pl.projetEdp.budgetSeniorRallonge != null ? pl.projetEdp.budgetSeniorRallonge : 0),

                                      //budgetDirectionTotale = (pl.projetEdp.budgetDirection != null ? pl.projetEdp.budgetDirection : 0) + (pl.projetEdp.budgetDirectionRallonge != null ? pl.projetEdp.budgetDirectionRallonge : 0),



                                      tte?.profileId,
                                      Monday = (Convert.ToDouble(tted?.Monday.Substring(0, 2)) + Convert.ToDouble(tted?.Monday.Substring(3, 2)) / 60) / 8,
                                      Tuesday = (Convert.ToDouble(tted?.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted?.Tuesday.Substring(3, 2)) / 60) / 8,
                                      Friday = (Convert.ToDouble(tted?.Friday.Substring(0, 2)) + Convert.ToDouble(tted?.Friday.Substring(3, 2)) / 60) / 8,
                                      Wednesday = (Convert.ToDouble(tted?.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted?.Wednesday.Substring(3, 2)) / 60) / 8,
                                      Thursday = (Convert.ToDouble(tted?.Thursday.Substring(0, 2)) + Convert.ToDouble(tted?.Thursday.Substring(3, 2)) / 60) / 8,


                                  }).ToList();




                    var result1 = (from edp in dbresult
                                   group edp by new
                                   {
                                       edp.profileId,

                                       edp.scope,
                                       edp.Id,
                                       edp.projectName,
                                       edp.StatusId,
                                       edp.StartDate,
                                       edp.PlannedDate,
                                       edp.DeliveryDate,
                                       edp?.client,
                                       edp.Nature,

                                       //edp.budgetDirectionTotale,
                                       //edp.budgetConfirmeTotale,
                                       //edp.budgetGPTotale,
                                       //edp.budgetJuniorTotale,
                                       //edp.budgetValidationTotale,
                                       //edp.budgetSeniorTotale,





                                   } into g
                                   select new
                                   {


                                       g.Key.profileId,
                                       g.Key.Nature,

                                       g.Key.projectName,
                                       g.Key.Id,
                                       g.Key.StatusId,
                                       g.Key.StartDate,
                                       g.Key.PlannedDate,
                                       g.Key.DeliveryDate,
                                       g.Key.client,
                                       g.Key.scope,


                                       //                coutEstime = (g.Key.profileId == "confirmé" ? g.Key.budgetConfirmeTotale
                                       //                 : (g.Key.profileId == "Direction" ? g.Key.budgetDirectionTotale
                                       //                 : (g.Key.profileId == "GP" ? g.Key.budgetGPTotale
                                       //                 : (g.Key.profileId == "junior" ? g.Key.budgetJuniorTotale
                                       //                 : (g.Key.profileId == "sénior" ? g.Key.budgetSeniorTotale
                                       //         : (g.Key.profileId == "Validation" ? g.Key.budgetValidationTotale
                                       //: 0)))))
                                       // ) * Convert.ToDouble(_context.profile.Where(x => x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault()),


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
                                           edp.client,
                                           edp.scope,
                                           edp.Nature,

                                       } into g
                                       select new
                                       {
                                           g.Key.Nature,
                                           g.Key.projectName,
                                           g.Key.Id,
                                           g.Key.StatusId,
                                           g.Key.StartDate,
                                           g.Key.PlannedDate,
                                           g.Key.DeliveryDate,
                                           g.Key.client,
                                           g.Key.scope,
                                           showDetails = false,
                                           coutEstime = 0,
                                           charge = g.Sum(x => x.charge),
                                           cout = g.Sum(x => x.cout)
                                       }).ToList();




                    return Ok(ResultFinal);


                }

            }
            catch (Exception e)
            {

                return BadRequest(e);
            }




        }


        [HttpGet("getStatDetailsByLivraison/{idProjet}")]
        public IActionResult getStatDetailsByLivraison(string idProjet)
        {

            var resultat = (from pl in _context.DetailLivraisons


                            where pl.ProjetLivraisonId == idProjet
                            select new
                            {
                                nom = pl.Projet.Nommenclature,
                                PlannedDate = pl.PlannedDate,
                                InitialPlannedDate = pl.InitialPlannedDate,

                                DeliveryDate = pl.DeliveryDate,
                                Delivery = pl.Delivery,
                                StartDate = pl.StartDate,
                                StatusId = pl.StatusId,
                                TTMId = pl.TTMId,
                                chargeConsommerTotale =pl.taches.Sum(x => Convert.ToDouble(x.chargeConsomme))

                            }).Distinct().ToList();



            return Ok(resultat);
        }


        // GET: api/ProjetLivraisons
        [HttpPost("searsh")]
        public  IActionResult GetProjetLivraisons(Search Search)
        {
            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           //join client in _context.Clients on projetLivraisons.ClientId equals client.Id

                           where
                           (Search.AssocitionProjetCompatble == 0 || (Search.AssocitionProjetCompatble == 1 && projetLivraisons.projetEdpId != null) || (Search.AssocitionProjetCompatble == 2 && projetLivraisons.projetEdpId == null)) &&
                             (Search.client == "0" || projetLivraisons.ClientId == Search.client)
                           && (Search.nature == "0" || projetLivraisons.Nature == Search.nature)

                           && (Search.status == "0" || projetLivraisons.StatusId == Search.status)

                       && ( projetLivraisons.DetailLivraisons.Select(x=>x.Projet).Where(x => x.Type == Search.type).Count()>0 || (Search.type == "0" && projetLivraisons.DetailLivraisons.Count==0)) 

                        
                           select new
                           {
                               withProjectComptable= projetLivraisons.projetEdpId!=null? true :false,
                               projetLivraisons.projetEdpId,
                               projetLivraisons.Nature,
                               ProjetName = projetLivraisons.ProjetName,
                               Id = projetLivraisons.Id,
                               InitialPlannedDate = projetLivraisons.InitialPlannedDate,
                               Modificateur = projetLivraisons.Modificateur,
                               Planned = projetLivraisons.Planned,
                               PlannedDate = projetLivraisons.PlannedDate,
                               StartDate = projetLivraisons.StartDate,
                               StatusId = projetLivraisons.StatusId,
                               TTMId = projetLivraisons.TTMId,
                               ClientId = projetLivraisons.ClientId,
                               Createur = projetLivraisons.Createur,
                               DateCreation = projetLivraisons.DateCreation,
                               DateDerniereModification = projetLivraisons.DateDerniereModification,
                               Delivery = projetLivraisons.Delivery,
                               DeliveryDate = projetLivraisons.DeliveryDate,
                               Description = projetLivraisons.Description,
                               EBRC = projetLivraisons.EBRC,
                               type = projetLivraisons.Type,
                               NomClient = projetLivraisons.Client.Nom,
                               countProjet = projetLivraisons.DetailLivraisons.Count()


                           }).ToList().OrderBy(x=> x.PlannedDate);
            return Ok ( resulat);


        }





        [HttpPost("satatByClientByComercial")]
        public IActionResult satatByClientByComercial(Search search)
        {
            try
            {
                var resultat = (from edp in _context.ProjetEdps
                                where (
                                (search.idClients.Count == 0 || search.idClients.Contains(edp.clientId))
                                && (String.IsNullOrEmpty(search.commercialId) || search.commercialId == edp.CommercialId)
                                && (String.IsNullOrEmpty(search.status) || search.status == edp.status)
                                   && (
                                   ((string.IsNullOrEmpty(search.status) || search.status == "Running") && edp.status == "Running")

                                || search.annees == edp.dateDebut.Value.Year
                                || search.annees == edp.dateFin.Value.Year
                                || search.annees == edp.dateFinInitial.Value.Year
                                || search.annees == edp.dateFinPrevue.Value.Year
                                || search.annees == Convert.ToInt32(edp.DateCreation.Substring(6, 4))
                                ))

                                select new
                                {
                                    chargeConsommerTotale = edp.ProjetLivraisons.SelectMany(x => x.DetailLivraisons).SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme)),
                               

                                    id = edp.id,
                                    commercantName = edp.Commercial.FullName,
                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                    natureCodeProjet = edp.CodeProjets.Nature,
                                    nom = edp.Nom,
                                    budgetInitial = edp.BudgetInitial,
                                    budgetRallonge = edp.BudgetRallonge,
                                    clientName = edp.client.Nom,
                                    dateCreation = Convert.ToDateTime(edp.DateCreation),
                                    edp.budgetSenior,
                                    edp.budgetSeniorRallonge,
                                    edp.status,

                                    edp.budgetConfirme,
                                    edp.budgetDirection,
                                    edp.budgetGP,
                                    edp.budgetJunior,
                                    edp.budgetValidation,
                                    budgetConfirmeTotale = edp.budgetConfirmeRallonge + edp.budgetConfirme,
                                    budgetGPTotale = edp.budgetGPRallonge + edp.budgetGP,
                                    budgetJuniorTotale = edp.budgetJuniorRallonge + edp.budgetJunior,
                                    budgetValidationTotale = edp.budgetValidationRallonge + edp.budgetValidation,
                                    budgetSeniorTotale = edp.budgetSeniorRallonge + edp.budgetSenior,

                                    edp.budgetConfirmeRallonge,
                                    edp.budgetDirectionRallonge,
                                    edp.budgetGPRallonge,

                                    edp.budgetJuniorRallonge,
                                    edp.budgetValidationRallonge,
                                    countProjetLivraison = edp.ProjetLivraisons.Count(),


                                 ProjetLivraison=   edp.ProjetLivraisons


                                }).Distinct().ToList();









                return Ok(resultat);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }




        }



        [HttpPost("GetProjetLivraisonsByClient")]
        public IActionResult GetProjetLivraisonsByClient(Search searsh)
        {
            var clients = (from client in _context.Clients
                           where

                 (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0) && client.ProjetLivraisons.Count > 0


                           select new
                           {
                               client = client.Nom,


                               filtredLivraison = client.ProjetLivraisons.OrderByDescending(x => x.PlannedDate.Date < DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
                                                       .ThenBy(x => x.PlannedDate.Date >= DateTime.Now.Date ? x.PlannedDate : DateTime.Now).Where(x => (searsh.types.Count == 0 || (x.DetailLivraisons.Count != 0 && x.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
                               && ((x.PlannedDate.Date >= searsh.startDate && x.PlannedDate.Date <= searsh.endDate) || (x.DeliveryDate != null && (x.DeliveryDate >= searsh.startDate && x.DeliveryDate <= searsh.endDate))))





                           });
            var resulat = clients?.Where(x => x.filtredLivraison.Count() > 0).Select(x => new
            {

                rowSpan = x.filtredLivraison.Count(),
                x.client,
                maxDate = x.filtredLivraison.Select(y => y.PlannedDate).First(),
                Livraisons = x.filtredLivraison.Select(z => new
                {
                    z.StartDate,
                    z.DateCreation,
                    z.DeliveryDate,
                    z.PlannedDate,
                    z.scope,
                    z.InitialPlannedDate,
                    z.ProjetName,
                    z.StatusId,
                    z.TTMId,
                    z.Delivery,
                    detailsCount = z.DetailLivraisons.Count,
                    z.Id,
                    showDetails = false,
                    z.Nature


                })

            });
            return Ok(new
            {
                projetLivraisons = resulat?.OrderByDescending(x => x.maxDate < DateTime.Now.Date ? x.maxDate : DateTime.Now)
                                       .ThenBy(x => x.maxDate.Date >= DateTime.Now.Date ? x.maxDate : DateTime.Now).ToList(),
                totaleLivrasion = resulat?.SelectMany(x => x.Livraisons).Count(),
                NotStarttedCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Not Started").Count(),
                RunningCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Running").Count(),
                CanceledCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Canceled").Count(),
                DeliveredCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Delivered").Count(),
                PostponedCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Postponed").Count(),
                lateCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "LATE" && x.StatusId == "Delivered").Count(),
                solutionCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.Nature == "Solution").Count(),
                tpeCount = resulat?.SelectMany(x => x.Livraisons).Where(x => x.Nature == "TPE").Count(),



                onTimeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "ON TIME" && x.StatusId == "Delivered").Count(),
                standByCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Stand By").Count()
            });

















            //            var resulat1 = (from projetLivraison in _context.ProjetLivraisons
            //                            join client in _context.Clients on projetLivraison.ClientId equals client.Id
            //                            where (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0)
            //                         && ((searsh.types.Count == 0 || (projetLivraison.DetailLivraisons.Count != 0 && projetLivraison.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
            //                          && ((projetLivraison.PlannedDate.Date >= searsh.startDate && projetLivraison.PlannedDate.Date <= searsh.endDate) || (projetLivraison.DeliveryDate != null && (projetLivraison.DeliveryDate >= searsh.startDate && projetLivraison.DeliveryDate <= searsh.endDate))))



            //                            select new
            //                            {
            //                                clinetName = client.Nom,

            //                                clinetId = client.Id,

            //                                projetLivraison

            //                            }).ToList();


            //            var group = (from r in resulat1
            //                         group r by (r.clinetId, r.clinetName) into g
            //                         select new
            //                         {
            //                             client= g.Key.clinetName,
            //                             Livraisons = g.Select(x => x.projetLivraison).OrderByDescending(x => x.PlannedDate.Date < DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
            //                             .ThenBy(x => x.PlannedDate.Date >= DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
            //                         });


            //           // return Ok(group);







            //            var resulat = group.Select(x => new
            //            {

            //                rowSpan = x.Livraisons.Count(),
            //                x.client,
            //                maxDate = x.Livraisons.Select(y => y.PlannedDate).First(),

            //                Livraisons = x.Livraisons.Select(z => new
            //{
            //  z.StartDate,
            //  z.DateCreation,
            //  z.DeliveryDate,
            //  z.PlannedDate,
            //  z.scope,
            //  z.InitialPlannedDate,
            //  z.ProjetName,
            //  z.StatusId,
            //  z.TTMId,
            //  z.Delivery,
            //  detailsCount = z.DetailLivraisons.Count,
            //  z.Id,
            //  showDetails = false,
            //  z.Nature


            //})

            //});


            //    return Ok(new
            //    {
            //        projetLivraisons = resulat.OrderByDescending(x => x.maxDate < DateTime.Now.Date ? x.maxDate : DateTime.Now).ToList(),
            //        totaleLivrasion = resulat.SelectMany(x => x.Livraisons).Count(),
            //        NotStarttedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Not Started").Count(),
            //        RunningCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Running").Count(),
            //        CanceledCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Canceled").Count(),
            //        DeliveredCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Delivered").Count(),
            //        PostponedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Postponed").Count(),
            //        lateCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "LATE" && x.StatusId == "Delivered").Count(),
            //        solutionCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "Solution").Count(),
            //        tpeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "TPE").Count(),



            //        onTimeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "ON TIME" && x.StatusId == "Delivered").Count(),
            //        standByCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Stand By").Count()
            //    });
            //var resulat = resulat1.Select(x => new
            //{
            //    x.maxDate,
            //    x.rowSpan,
            //    x.client,
            //    // x.PreviousLivraisons.Concat(x.NextLivraisons)
            //});


            //    resulat1.SelectMany(x=>x.NextLivraisons).

            //            var clients = (from    client in _context.Clients  
            //                           where

            //                 (  


            //                           select new
            //                           {
            //                               client = client.Nom,


            //                               filtredLivraison = client.ProjetLivraisons.Where(x =>  (searsh.types.Count == 0 || (x.DetailLivraisons.Count != 0 && x.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0)) 
            //                               && ((x.PlannedDate.Date >= searsh.startDate && x.PlannedDate.Date <= searsh.endDate) || (x.DeliveryDate!=null &&(x.DeliveryDate >= searsh.startDate && x.DeliveryDate <= searsh.endDate)))

            //                          ),



            //                           });
            //            var resulat = from projetLivraison in _context.ProjetLivraisons
            //                          join client in _context.Clients on projetLivraison.ClientId equals client.Id

            //                          where (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0)
            //                          &&       ((searsh.types.Count == 0 || (projetLivraison.DetailLivraisons.Count != 0 && projetLivraison.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
            //                           && ((projetLivraison.PlannedDate.Date >= searsh.startDate && projetLivraison.PlannedDate.Date <= searsh.endDate) || (projetLivraison.DeliveryDate != null && (projetLivraison.DeliveryDate >= searsh.startDate && projetLivraison.DeliveryDate <= searsh.endDate))))
            //                          group projetLivraison by (.projetId) into g2



        }








        // [HttpPost("GetProjetLivraisonsByClient")]
        // public IActionResult GetProjetLivraisonsByClient(Search searsh)
        // {




        //     //var clients = (from client in _context.Clients
        //     //               where

        //     //     (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0) && client.ProjetLivraisons.Count > 0


        //     //               select new
        //     //               {
        //     //                   client = client.Nom,

        //     //                   filtredLivraison=  client.ProjetEdps.SelectMany(x=>x.ProjetLivraisons).OrderByDescending(x => x.PlannedDate.Date < DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
        //     //                                           .ThenBy(x => x.PlannedDate.Date >= DateTime.Now.Date ? x.PlannedDate : DateTime.Now).Where(x => (searsh.types.Count == 0 || (x.DetailLivraisons.Count != 0 && x.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
        //     //                   && ((x.PlannedDate.Date >= searsh.startDate && x.PlannedDate.Date <= searsh.endDate) || (x.DeliveryDate != null && (x.DeliveryDate >= searsh.startDate && x.DeliveryDate <= searsh.endDate))))

        //     //               });






        //     //var ProjetLivv = _context.ProjetLivraisons



        //     //.OrderByDescending(x => x.PlannedDate.Date < DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
        //     //                                            .ThenBy(x => x.PlannedDate.Date >= DateTime.Now.Date ? x.PlannedDate : DateTime.Now).Where(x => (searsh.types.Count == 0 || (x.DetailLivraisons.Count != 0 && x.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
        //     //                    && ((x.PlannedDate.Date >= searsh.startDate && x.PlannedDate.Date <= searsh.endDate) || (x.DeliveryDate != null && (x.DeliveryDate >= searsh.startDate && x.DeliveryDate <= searsh.endDate))))


        //     //var charge = (from pl in _context.ProjetLivraisons
        //     //              join d in _context.DetailLivraisons on pl.Id equals d.ProjetLivraisonId
        //     //              join t in _context.Taches on d.Id equals t.detailLivraisonId
        //     //              join u in _context.profileUser on t.UserId equals u.userId
        //     //         group t by u.profileId into g
        //     //              select new
        //     //              {

        //     //                  g.Key,
        //     //                  chargeConsomme = g.Sum(x=>Convert.ToDouble(x.chargeConsomme))

        //     //              }).ToList();


        //     //var projetComptable = (from c in _context.Clients
        //     //                       join lv in _context.ProjetLivraisons on c.Id equals lv.ClientId

        //     //                       join l in _context.ProjetEdps on lv.projetEdpId equals l.id
        //     //                       join d in _context.DetailLivraisons on lv.Id equals d.ProjetLivraisonId
        //     //                       join t in _context.Taches on d.Id equals t.detailLivraisonId
        //     //                       join p in _context.profileUser on t.UserId equals p.userId
        //     //                       group new { lv, t, c } by new { c.Id, c.Nom, l.id, l.budgetConfirme, l.budgetConfirmeRallonge, l.budgetDirection, l.budgetDirectionRallonge, l.budgetGP, l.budgetGPRallonge, l.budgetJuniorRallonge, l.budgetJunior, l.budgetSenior, l.budgetSeniorRallonge, l.budgetValidation, l.budgetValidationRallonge } into g
        //     //                       select new
        //     //                       {


        //     //                           g.Key.Nom,
        //     //                           budgetValidation = g.Key.budgetValidationRallonge + g.Key.budgetValidation,
        //     //                           budgetSenior = g.Key.budgetSenior + g.Key.budgetSeniorRallonge,
        //     //                           budgetJunior = g.Key.budgetJunior + g.Key.budgetJuniorRallonge,
        //     //                           budgetGP = g.Key.budgetGP + g.Key.budgetGPRallonge,
        //     //                           budgetDirection = g.Key.budgetDirection + g.Key.budgetDirectionRallonge,

        //     //                           g.Select(x => x.lv)

        //     //                       }); .ToListAsync();


        //     //var groupbyprojetComptable = (from l in _context.ProjetLivraisons

        //     //                              select new
        //     //                              {



        //     //                              }).ToListAsync();







        //                                                                                                    //select new
        //                                                                                                    //{
        //                                                                                                    //    g.Key,
        //                                                                                                    //  //charge=  g.Sum(x => Convert.ToDouble(x))

        //                                                                                                    //})).ToList()



        //                                                                                //group t by t.profileId into g
        //                                                                                //select new
        //                                                                                //{
        //                                                                                //    // key="1",
        //                                                                                //    // chargeConsomme="10"
        //                                                                                //    //g.Key,
        //                                                                                //    //chargeConsomme = g.Sum(x => Convert.ToDouble(x.chargeConsomme))

        //                                                                                //}).ToList(),

        //                                                                                /*************************************************
        //                                                                                                                                                              chargeConsommerTotale =


        //                                                                                                                                                              p.ProjetLivraisons.SelectMany(x => x.DetailLivraisons).SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme)),


        //                                                                                                                                                              commercantName = p.Commercial.FullName,
        //                                                                                                                                                              CodeProjet = p.CodeProjets.Numero + "_" + p.CodeProjet,
        //                                                                                                                                                              natureCodeProjet = p.CodeProjets.Nature,

        //                                                                                                                                                              p.Nom,
        //                                                                                                                                                              client = p.client.Nom,
        //                                                                                                                                                              p.status,
        //                                                                                                                                                              budgettotale = p.BudgetInitial + p.BudgetRallonge,

        //                                                                                                                                                              budgetConfirmetotale = p.budgetConfirme + p.budgetConfirmeRallonge,
        //                                                                                                                                                              budgetDirectiontotale = p.budgetDirection + p.budgetDirectionRallonge,
        //                                                                                                                                                              budgetJuniortotale = p.budgetJunior + p.budgetJuniorRallonge,
        //                                                                                                                                                              budgetSeniortotale = p.budgetSenior + p.budgetSeniorRallonge,
        //                                                                                                                                                              budgetValidationtotale = p.budgetValidation + p.budgetValidationRallonge,
        //                                                                                                                                                          }).First(),
        //                                                                                                                                        RowSpanLivraison=  group2.Count(),
        //                                                                                  *********************/

        //                         //                                          }).ToList()

        //                         //                   }
        //                         //};




        ////     var resulat = groupbyClient.Select(x => new
        //  //   {

        //         //rowSpan = x.livraisons.Count(),
        //         //x.client,
        //        // maxDate = x.livraisons.Select(y => y.livraison).First(),
        //         //Livraisons = x.livraisons.Select(z => new
        //         //{
        //         //    z.StartDate,
        //         //    z.DateCreation,
        //         //    z.DeliveryDate,
        //         //    z.PlannedDate,
        //         //    z.scope,
        //         //    z.InitialPlannedDate,
        //         //    z.ProjetName,
        //         //    z.StatusId,
        //         //    z.TTMId,
        //         //    z.Delivery,
        //         //    z.projetEdpId,
        //         //    z.projetEdp,
        //         //    detailsCount = z.DetailLivraisons.Count,
        //         //    z.Id,
        //         //    showDetails = false,
        //         //    z.Nature,

        //         //})

        // //    }
        //     return Ok();




        //     //return Ok(new
        //     //{
        //     //    projetLivraisons = resulat.OrderByDescending(x => x.maxDate < DateTime.Now.Date ? x.maxDate : DateTime.Now)
        //     //                           .ThenBy(x => x.maxDate.Date >= DateTime.Now.Date ? x.maxDate : DateTime.Now).ToList(),
        //     //    totaleLivrasion = resulat.SelectMany(x => x.Livraisons).Count(),
        //     //    NotStarttedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Not Started").Count(),
        //     //    RunningCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Running").Count(),
        //     //    CanceledCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Canceled").Count(),
        //     //    DeliveredCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Delivered").Count(),
        //     //    PostponedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Postponed").Count(),
        //     //    lateCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "LATE" && x.StatusId == "Delivered").Count(),
        //     //    solutionCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "Solution").Count(),
        //     //    tpeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "TPE").Count(),
        //     //    onTimeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "ON TIME" && x.StatusId == "Delivered").Count(),
        //     //    standByCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Stand By").Count()
        //     //});

















        //     //            var resulat1 = (from projetLivraison in _context.ProjetLivraisons
        //     //                            join client in _context.Clients on projetLivraison.ClientId equals client.Id
        //     //                            where (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0)
        //     //                         && ((searsh.types.Count == 0 || (projetLivraison.DetailLivraisons.Count != 0 && projetLivraison.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
        //     //                          && ((projetLivraison.PlannedDate.Date >= searsh.startDate && projetLivraison.PlannedDate.Date <= searsh.endDate) || (projetLivraison.DeliveryDate != null && (projetLivraison.DeliveryDate >= searsh.startDate && projetLivraison.DeliveryDate <= searsh.endDate))))



        //     //                            select new
        //     //                            {
        //     //                                clinetName = client.Nom,

        //     //                                clinetId = client.Id,

        //     //                                projetLivraison

        //     //                            }).ToList();


        //     //            var group = (from r in resulat1
        //     //                         group r by (r.clinetId, r.clinetName) into g
        //     //                         select new
        //     //                         {
        //     //                             client= g.Key.clinetName,
        //     //                             Livraisons = g.Select(x => x.projetLivraison).OrderByDescending(x => x.PlannedDate.Date < DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
        //     //                             .ThenBy(x => x.PlannedDate.Date >= DateTime.Now.Date ? x.PlannedDate : DateTime.Now)
        //     //                         });


        //     //           // return Ok(group);







        //     //            var resulat = group.Select(x => new
        //     //            {

        //     //                rowSpan = x.Livraisons.Count(),
        //     //                x.client,
        //     //                maxDate = x.Livraisons.Select(y => y.PlannedDate).First(),

        //     //                Livraisons = x.Livraisons.Select(z => new
        //     //{
        //     //  z.StartDate,
        //     //  z.DateCreation,
        //     //  z.DeliveryDate,
        //     //  z.PlannedDate,
        //     //  z.scope,
        //     //  z.InitialPlannedDate,
        //     //  z.ProjetName,
        //     //  z.StatusId,
        //     //  z.TTMId,
        //     //  z.Delivery,
        //     //  detailsCount = z.DetailLivraisons.Count,
        //     //  z.Id,
        //     //  showDetails = false,
        //     //  z.Nature


        //     //})

        //     //});


        //     //    return Ok(new
        //     //    {
        //     //        projetLivraisons = resulat.OrderByDescending(x => x.maxDate < DateTime.Now.Date ? x.maxDate : DateTime.Now).ToList(),
        //     //        totaleLivrasion = resulat.SelectMany(x => x.Livraisons).Count(),
        //     //        NotStarttedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Not Started").Count(),
        //     //        RunningCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Running").Count(),
        //     //        CanceledCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Canceled").Count(),
        //     //        DeliveredCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Delivered").Count(),
        //     //        PostponedCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Postponed").Count(),
        //     //        lateCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "LATE" && x.StatusId == "Delivered").Count(),
        //     //        solutionCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "Solution").Count(),
        //     //        tpeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.Nature == "TPE").Count(),



        //     //        onTimeCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.TTMId == "ON TIME" && x.StatusId == "Delivered").Count(),
        //     //        standByCount = resulat.SelectMany(x => x.Livraisons).Where(x => x.StatusId == "Stand By").Count()
        //     //    });
        //     //var resulat = resulat1.Select(x => new
        //     //{
        //     //    x.maxDate,
        //     //    x.rowSpan,
        //     //    x.client,
        //     //    // x.PreviousLivraisons.Concat(x.NextLivraisons)
        //     //});


        //     //    resulat1.SelectMany(x=>x.NextLivraisons).

        //     //            var clients = (from    client in _context.Clients  
        //     //                           where

        //     //                 (  


        //     //                           select new
        //     //                           {
        //     //                               client = client.Nom,


        //     //                               filtredLivraison = client.ProjetLivraisons.Where(x =>  (searsh.types.Count == 0 || (x.DetailLivraisons.Count != 0 && x.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0)) 
        //     //                               && ((x.PlannedDate.Date >= searsh.startDate && x.PlannedDate.Date <= searsh.endDate) || (x.DeliveryDate!=null &&(x.DeliveryDate >= searsh.startDate && x.DeliveryDate <= searsh.endDate)))

        //     //                          ),



        //     //                           });
        //     //            var resulat = from projetLivraison in _context.ProjetLivraisons
        //     //                          join client in _context.Clients on projetLivraison.ClientId equals client.Id

        //     //                          where (searsh.idClients.Contains(client.Id) || searsh.idClients.Count == 0)
        //     //                          &&       ((searsh.types.Count == 0 || (projetLivraison.DetailLivraisons.Count != 0 && projetLivraison.DetailLivraisons.Select(z => z.Projet).Where(y => searsh.types.Contains(y.Type)).Count() > 0))
        //     //                           && ((projetLivraison.PlannedDate.Date >= searsh.startDate && projetLivraison.PlannedDate.Date <= searsh.endDate) || (projetLivraison.DeliveryDate != null && (projetLivraison.DeliveryDate >= searsh.startDate && projetLivraison.DeliveryDate <= searsh.endDate))))
        //     //                          group projetLivraison by (.projetId) into g2



        // }

        [HttpGet("GetProjetLivraisonByUser/{iduser}")]
        public IActionResult GetProjetLivraisonByUser(string iduser)
        {

            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where userEquipe.UserId == iduser
                           group new { projetLivraisons } by new { projetLivraisons.ProjetName, projetLivraisons.Delivery, projetLivraisons.Id } into grp
                           select new
                           {

                               ProjetName = grp.Key.ProjetName + "  V" + grp.Key.Delivery,
                               Id = grp.Key.Id,

                           }).ToList();
            return Ok(resulat);


        }




        [HttpGet("GetProjetLivraisonByUser2weeksBefore/{iduser}")]
        public IActionResult GetProjetLivraisonByUser2weeksBefore(string iduser)
        {
            var date = DateTime.Now.AddDays(-21);

            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where userEquipe.UserId == iduser && (projetLivraisons.DeliveryDate>=date.Date|| projetLivraisons.DeliveryDate==null)
                           group new { projetLivraisons } by new { projetLivraisons.ProjetName, projetLivraisons.Id , projetLivraisons.Delivery} into grp
                           select new
                           {

                               ProjetName = grp.Key.ProjetName +"  V"+ grp.Key.Delivery,
                               Id = grp.Key.Id,

                           }).ToList();

            return Ok(resulat);
        }


        [HttpGet("GetProjetLivraisonByEquipe/{idEquipe}")]
        public IActionResult GetProjetLivraisonByEquipe(string idEquipe)
        {

            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           where ProjetEquipes.EquipeId == idEquipe 
                           group new { projetLivraisons } by new { projetLivraisons.ProjetName, projetLivraisons.Id, projetLivraisons.Delivery } into grp
                           select new
                           {

                               ProjetName = grp.Key.ProjetName + "  V" + grp.Key.Delivery,
                               Id = grp.Key.Id,

                           }).ToList();
              return Ok(resulat);


        }


        [HttpGet("GetProjetLivraisonForTasks/{idUser}")]
        public IActionResult GetProjetLivraisonForTasks(string idUser)
        {

            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join taches in _context.Taches on detailLivraisons.Id equals taches.detailLivraisonId
                            join project in _context.Projets on detailLivraisons.ProjetId equals project.Id
                             where( taches.UserId==idUser || project.Publique=="true" ) && taches.status!="3"
               
                           select new
                           {

                               ProjetName = projetLivraisons.ProjetName + "  V" + projetLivraisons.Delivery,
                               Id = projetLivraisons.Id,

                           }).ToList();
            return Ok(resulat);


        }



        [HttpGet("GetProjetLivraisonByManager2weeksBefore/{idManager}")]
        public IActionResult GetProjetLivraisonByManager2weeksBefore(string idManager)
        {
            var date = DateTime.Now.AddDays(-21);


            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id

                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join equipe in _context.Equips on ProjetEquipes.EquipeId equals equipe.Id

                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where (userEquipe.UserId == idManager || equipe.ManagerId == idManager) && (projetLivraisons.DeliveryDate >= date.Date || projetLivraisons.DeliveryDate == null)
                           // where ProjetEquipes.Equipe.ManagerId == idManager && project.lockoutEnabled == true
                           group new { projetLivraisons } by new { projetLivraisons.ProjetName, projetLivraisons.Id, projetLivraisons.Delivery } into grp
                           select new
                           {

                               ProjetName = grp.Key.ProjetName + "  V" + grp.Key.Delivery,
                               Id = grp.Key.Id,

                           }).Distinct().ToList();
            return Ok(resulat);
        }
        
        [HttpGet("GetProjetLivraisonByManager/{idManager}")]
        public IActionResult GetProjetLivraisonByManager(string idManager)
        {


            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                           join detailLivraisons in _context.DetailLivraisons on projetLivraisons.Id equals detailLivraisons.ProjetLivraisonId
                           join project in _context.Projets on detailLivraisons.ProjetId equals project.Id

                           join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                           join equipe in _context.Equips on ProjetEquipes.EquipeId equals equipe.Id

                           join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                           where (userEquipe.UserId == idManager || equipe.ManagerId == idManager)
                           // where ProjetEquipes.Equipe.ManagerId == idManager && project.lockoutEnabled == true
                           group new { projetLivraisons } by new { projetLivraisons.ProjetName, projetLivraisons.Id ,projetLivraisons.Delivery} into grp
                           select new
                           {

                               ProjetName = grp.Key.ProjetName + "  V" + grp.Key.Delivery,
                               Id = grp.Key.Id,

                           }).Distinct().ToList();
            return Ok(resulat);


        }

        [HttpGet("GetProjetLivraisonForAdminForTaches2weeksBefore")]
        public IActionResult GetProjetLivraisonForAdminForTaches2weekslater()
        {

            var date = DateTime.Now.AddDays(-21);
            var resulat = (from projetLivraisons in _context.ProjetLivraisons

                           where (projetLivraisons.DeliveryDate>= date.Date || projetLivraisons.DeliveryDate == null)

                           select new
                           {

                               ProjetName = projetLivraisons.ProjetName + "  v"+projetLivraisons.Delivery,
                               Id = projetLivraisons.Id,

                           }).ToList();
            return Ok(resulat);


        }

        [HttpGet("GetProjetLivraisonForAdminForTaches")]
        public IActionResult GetProjetLivraisonForAdminForTaches()
        {

            var resulat = (from projetLivraisons in _context.ProjetLivraisons
                    
                         

                           select new
                           {

                               ProjetName = projetLivraisons.ProjetName + "  v" + projetLivraisons.Delivery,
                               Id = projetLivraisons.Id,

                           }).ToList();
            return Ok(resulat);


        }


        // GET: api/ProjetLivraisons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjetLivraison(string id)
        {
            var projetLivraison = await _context.ProjetLivraisons.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.DeliveryDate,
                x.scope,
                x.Createur,
                x.ClientId,
                x.ProjetName,
                x.DateCreation,
                x.Delivery,
                x.Description,
                x.EBRC,
                x.InitialPlannedDate,
                x.Nature,
                x.Planned,
                x.PlannedDate,
                x.projetEdpId,
                x.StartDate,
                x.StatusId,
                x.Type,
                x.Commentaires,
                x.TTMId,
                x.lockoutEnabled,
                x.Modificateur,
                x.DateDerniereModification,
                projetEdpNom=  x.projetEdp.Nom,
                clientProjetCompatable = x.projetEdp.clientId,
                Listdelivarydates = x.DetailLivraisons.Select(x=>x.DeliveryDate).ToList(),
                ListdeStartdates = x.DetailLivraisons.Select(x=>x.StartDate).ToList(),
                listStatus = x.DetailLivraisons.Select(x => x.StatusId).ToList()
            }).FirstOrDefaultAsync() ;

            if (projetLivraison == null)
            {
                return NotFound();
            }
            return Ok(projetLivraison);
        }

        [HttpPost("details/{id}")]
        public async Task<ActionResult> GetProjetLivraisonDetails(string id )
        {

            //changes here by wiss
            try
            {
                var result = await _context.DetailLivraisons.Include(x => x.Projet).
     Where(x => x.ProjetLivraisonId == id)

     .Select(x => new
     {
         x.ProjetLivraisonId,
         x.Createur,
         x.DateCreation,
         x.Id,
         x.Description,
         x.Delivery,
         x.Planned,
         x.EBRC,
         x.Type,
         x.StartDate,
         x.PlannedDate,
         x.InitialPlannedDate,
         x.DeliveryDate,
         x.TTMId,
         x.StatusId,
         x.ProjetId,
         projetName = x.Projet.Nommenclature,
         countTache = x.taches.Count()
     }).ToListAsync();

                return Ok(result);
            }
            catch (Exception e )
            {

                return Ok(e);
            }
            
 
        }

        [HttpGet("GetProjetLivraisonDetailsByUser/{id}/{idUser}")]
        public async Task<ActionResult> GetProjetLivraisonDetailsByUser(string id, string idUser)
        {
            // var result = await _context.DetailLivraisons.Where(x => x.ProjetLivraisonId == id).ToListAsync();
            // return Ok(result);
            var result = await (from dl in _context.DetailLivraisons
                                join p in _context.Projets on dl.ProjetId equals p.Id
                                join pe in _context.ProjetEquipes on p.Id equals pe.ProjetId
                                join eu in _context.EquipeUser on pe.EquipeId equals eu.EquipeId
                                where dl.ProjetLivraisonId == id && eu.UserId == idUser
                                select new
                                //await _context.DetailLivraisons.Include(x => x.Projet).ThenInclude(x => x.projetsEquipe).
                                //Where(x => x.ProjetLivraisonId == id && x.Projet.projetsEquipe.).Select(x => new
                                {dl.Id,
                                    dl.ProjetLivraisonId,
                                    dl.Createur,
                                    dl.DateCreation,
                                    dl.Description,
                                    dl.Delivery,
                                    dl.Planned,
                                    dl.EBRC,
                                    dl.Type,
                                    dl.StartDate,
                                    dl.PlannedDate,
                                    dl.InitialPlannedDate,
                                    dl.DeliveryDate,
                                    dl.TTMId,
                                    dl.StatusId,
                                    dl.ProjetId,
                                    projetName = p.Nommenclature,
                                    countTache = dl.taches.Count()
                                }).Distinct().ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetProjetLivraisonDetailsByEquipe/{id}/{idEquipe}")]
        public async Task<ActionResult> GetProjetLivraisonDetailsByEquipe(string id, string idEquipe)
        {
            // var result = await _context.DetailLivraisons.Where(x => x.ProjetLivraisonId == id).ToListAsync();
            // return Ok(result);
            var result =await ( from dl in _context.DetailLivraisons 
                         join p in _context.Projets on dl.ProjetId equals p.Id
                         join pe in _context.ProjetEquipes on p.Id equals pe.ProjetId
                         where dl.ProjetLivraisonId==id && pe.EquipeId==idEquipe
                         select new

                //await _context.DetailLivraisons.Include(x => x.Projet).ThenInclude(x => x.projetsEquipe).
                //Where(x => x.ProjetLivraisonId == id && x.Projet.projetsEquipe.).Select(x => new
                {
                             dl.Id,
                             dl.ProjetLivraisonId,
                             dl.Createur,
                             dl.DateCreation,
                             dl.Description,
                             dl.Delivery,
                    dl.Planned,
                    dl.EBRC,
                    dl.Type,
                    dl.StartDate,
                    dl.PlannedDate,
                    dl.InitialPlannedDate,
                    dl.DeliveryDate,
                    dl.TTMId,
                    dl.StatusId,
                    dl.ProjetId,
            projetName= p.Nommenclature,
                    countTache = dl.taches.Count()
                }).ToListAsync();

            return Ok(result);
        }


        [HttpGet("GetProjetLivraisonDetailsByManager/{id}/{idManager}")]
        public async Task<ActionResult> GetProjetLivraisonDetailsByManager(string id, string idManager)
        {
            // var result = await _context.DetailLivraisons.Where(x => x.ProjetLivraisonId == id).ToListAsync();
            // return Ok(result);
            //var result = await (from dl in _context.DetailLivraisons
            //                        // join t in _context.Taches on dl.Id equals t.detailLivraisonId
            //                    join p in _context.Projets on dl.ProjetId equals p.Id
            //                    join pe in _context.ProjetEquipes on p.Id equals pe.ProjetId
            //                    where dl.ProjetLivraisonId == id && pe.Equipe.ManagerId == idManager
            //                    group new { dl, p } by new
            //                    {
            //                        dl.Id,
            //                        dl.Description,
            //                        dl.Delivery,
            //                        dl.Planned,
            //                        dl.EBRC,
            //                        dl.Type,
            //                        dl.StartDate,
            //                        dl.PlannedDate,
            //                        dl.InitialPlannedDate,
            //                        dl.DeliveryDate,
            //                        dl.TTMId,
            //                        dl.StatusId,
            //                        dl.ProjetId,
            //                        p.Nommenclature,
            //                        //    taches=   dl.taches.DefaultIfEmptyjh()

            //                        //  t.UserId
            //                    } into grp

            //                    select new
            //                    {
            //                        // countTache = grp.Key.taches.Count(),
            //                        grp.Key.Id,
            //                        grp.Key.Description,
            //                        grp.Key.Delivery,
            //                        grp.Key.Planned,
            //                        grp.Key.EBRC,
            //                        grp.Key.Type,
            //                        grp.Key.StartDate,
            //                        grp.Key.PlannedDate,
            //                        grp.Key.InitialPlannedDate,
            //                        grp.Key.DeliveryDate,
            //                        grp.Key.TTMId,
            //                        grp.Key.StatusId,
            //                        grp.Key.ProjetId,
            //                        projetName = grp.Key.Nommenclature

            //                    }).ToListAsync();



            var result =await (from dl in _context.DetailLivraisons
                          //join p in _context.Projets on dl.ProjetId equals p.Id
                          //join pe in _context.ProjetEquipes on p.Id equals pe.ProjetId
                          //join userEquipe in _context.EquipeUser on pe.EquipeId equals userEquipe.EquipeId
                          where dl.ProjetLivraisonId == id
                          //&& (pe.Equipe.ManagerId == idManager || userEquipe.UserId == idManager) 
                               select new

                          //await _context.DetailLivraisons.Include(x => x.Projet).ThenInclude(x => x.projetsEquipe).
                          //Where(x => x.ProjetLivraisonId == id && x.Projet.projetsEquipe.).Select(x => new
                          {
                               isManager=dl.Projet.projetsEquipe.Select(x=>x.Equipe).Where(x=>x.ManagerId== idManager).Count(),
                               isUser= dl.Projet.projetsEquipe.Select(x => x.Equipe).SelectMany(x=>x.equipeUsers).Where(x=>x.UserId== idManager).Count(),
                              dl.Id,
                              dl.ProjetLivraisonId,
                              dl.Createur,
                              dl.DateCreation,
                              dl.Description,
                              dl.Delivery,
                              dl.Planned,
                              dl.EBRC,
                              dl.Type,
                              dl.StartDate,
                              dl.PlannedDate,
                              dl.InitialPlannedDate,
                              dl.DeliveryDate,
                              dl.TTMId,
                              dl.StatusId,
                              dl.ProjetId,
                      projetName= dl.Projet.Nommenclature,
                              countTache = dl.taches.Count()
                          }).Distinct().ToListAsync();
            return Ok(result);
        }
 
        [HttpPost("details")]
        public async Task<ActionResult> PostProjectDetails(DetailLivraison model)
        {
            
            if (string.IsNullOrEmpty(model.Id))
            {


            //         if (model.DeliveryDate != null)
            //{
            //    if (model.DeliveryDate?.Date > model.InitialPlannedDate?.Date)
            //    {
            //        model.TTMId = "LATE";
            //    }
            //    else if (model.DeliveryDate?.Date <= model.InitialPlannedDate?.Date)
            //        model.TTMId = "ONTIME";
            //}
                model.DateCreation = DateTime.Now.ToString();

                model.Id = Guid.NewGuid().ToString();
                _context.DetailLivraisons.Add(model);
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return Ok(model);
        }

       [HttpPost("Delete/details/{id}")]
        public async Task<ActionResult> DeleteProjectDetails(string id)
        {
            var entry = await _context.DetailLivraisons.FindAsync(id);
            if (entry != null)
            {
                _context.DetailLivraisons.Remove(entry);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // PUT: api/ProjetLivraisons/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("Update/{id}")]
        //public async Task<IActionResult> PutProjetLivraison(string id, ProjetLivraison projetLivraison)
        //{
        //    if (id != projetLivraison.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(projetLivraison).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProjetLivraisonExists(id))
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

        // POST: api/ProjetLivraisons
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProjetLivraison>> PostProjetLivraison(ProjetLivraison model)
        {

        

            //if (model.DeliveryDate != null)
            //{
            //    if (DateTime.Parse(model.DeliveryDate).Date > DateTime.Parse(model.InitialPlannedDate).Date)
            //    {
            //        model.TTMId = "LATE";
            //    }
            //    else if (DateTime.Parse(model.DeliveryDate).Date <= DateTime.Parse(model.InitialPlannedDate).Date)
            //        model.TTMId = "ONTIME";
            //}

            if (string.IsNullOrEmpty(model.Id))
            {
                model.DateCreation = DateTime.Now.ToString();

                _context.ProjetLivraisons.Add(model);
            }
            else
            {

                model.DateDerniereModification = DateTime.Now.ToString();
                _context.Entry(model).State = EntityState.Modified;

            }
           
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProjetLivraison", new { id = model.Id }, model);
            //if (model.Id == "")
            //{


            // _context.ProjetLivraisons.Add(model);


            //}
            //else
            // {
            // update the entire object
            // _context.ProjetLivraisons.Update(model);

            // update specific properties
            /*var entry = await _context.ProjetLivraisons.FirstOrDefaultAsync(x => x.Id == model.Id);
            entry.Description = model.Description;
            entry.Delivery = model.Delivery;*/
            // }


            //return CreatedAtAction("GetProjetLivraison", new { id = model.Id }, model);

        }

        // DELETE: api/ProjetLivraisons/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<ProjetLivraison>> DeleteProjetLivraison(string id)
        {
            var projetLivraison = await _context.ProjetLivraisons.Include(c=>c.DetailLivraisons).Where(c=>c.Id==id).FirstOrDefaultAsync();
            if (projetLivraison == null)
            {
                return NotFound();
            }
            _context.DetailLivraisons.RemoveRange(projetLivraison.DetailLivraisons);
            _context.ProjetLivraisons.Remove(projetLivraison);
            await _context.SaveChangesAsync();

            return projetLivraison;
        }
        [NonAction]
        public IEnumerable<DetailLivraison> getDtail(string ProjetLivraisonId)
        {
           return  _context.DetailLivraisons.Where(x => x.ProjetLivraisonId == ProjetLivraisonId);
            

        }

        //delete project and its detail
        //[HttpDelete("deletePd/{id}")]
        //public async Task<ActionResult<IEnumerable<DetailLivraison>>> DeleteProjetLivraisonDetail(string ProjetLivraisonId)
        //{
        //    DetailLivraison result = await _context.DetailLivraisons.Include(x => x.Projet).ToListAsync();

        //    result.RemoveAll(x => x.ProjetLivraisonId == ProjetLivraisonId);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //    var detailLivraison = await _context.DetailLivraisons.FindAsync(id);
        //    if (detailLivraison == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DetailLivraisons.Remove(detailLivraison);
        //    await _context.SaveChangesAsync();

        //    return detailLivraison;


        //}
        [NonAction]
        private bool ProjetLivraisonExists(string id)
        {
            return _context.ProjetLivraisons.Any(e => e.Id == id);
        }

        //treegrid
        [HttpGet("full3")]
        public async Task<ActionResult<IEnumerable<ProjetLivraison>>> GetFullProjetLivraisons()
        {
            var result = await _context.ProjetLivraisons.Include(p => p.DetailLivraisons).ToListAsync();
            return result;
        }


        [HttpGet("full")]
        public async Task<IActionResult> GetFulProjetLivraisons()
        {
            var result = await _context.ProjetLivraisons.Include(p => p.DetailLivraisons)
             .Select(s => new
             {
                 s.Id,
                  Project = s.Description,
                 s.Delivery,
                 s.Planned,
                 s.EBRC,
                 s.Type,
                 s.StartDate,
                 s.InitialPlannedDate,
                 s.PlannedDate,
                 s.DeliveryDate,
                 TTM = s.TTMId,
                 Status = s.StatusId,
                 details=s.DetailLivraisons.Select(c => new {
                     c.Id,
                     Project = c.Description,
                     c.Delivery,
                     c.Planned,
                     c.EBRC,
                     c.Type,
                     c.StartDate,
                     c.InitialPlannedDate,
                     c.PlannedDate,
                     c.DeliveryDate,
                     TTM = c.TTMId,
                     Status = c.StatusId}).ToList()
             }).ToListAsync();
            return Ok (result);
        }

       // }

        [HttpGet("full2")]
        public ActionResult<IQueryable<ProjetLivraison>> GetSousProjetLivraisonforTree()
        {
            var sousprojetLivraison = _context.ProjetLivraisons.Select(s => new
            {
                s.Id,
                s.Description,
                s.Delivery,
                s.Planned,
                s.EBRC,
                s.Type,
                s.StartDate,
                s.InitialPlannedDate,
                s.PlannedDate,
                s.DeliveryDate,
                s.TTMId,
                s.StatusId

                
            });

            return Ok(sousprojetLivraison);
        }



        //[HttpPost("clients")]
        //public async Task<IActionResult> UpdateClients(string projectId, [FromBody] List<string> clients)
        //{
        //    foreach (var item in clients)
        //    {
        //        _context.ProjetLivraisonClients.Add(new ProjetLivraisonClient
        //        {
        //            ProjetLivraisonId = projectId,
        //            ClientId = item
        //        });
        //    }
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[HttpPost("clients")]
        //public async Task<IActionResult> UpdateClients(string projectId, List<Client> clients)
        //{
        //    foreach (var item in clients)
        //    {
        //        _context.ProjetLivraisonClients.Add(new ProjetLivraisonClient
        //        {
        //            ProjetLivraisonId = projectId,
        //            ClientId = item.Id
        //        });
        //    }
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}




        [HttpPost("LivraisonStatus")]
        public async Task<IActionResult>  ProjetLivraisonStatus(SecondSearch search)
        {
            var CountTotal = 0;
            var CountDone = 0;
            var CountDate1Semaine = 0;
            var CountDate2Semaine = 0;
            var CountDate4Semaine = 0;
            var CountDate5Semaine = 0;
         
            List<ProjetLivraison> result = new List<ProjetLivraison>();
            if(search.idClients==null)
            {
                search.idClients = new List<string>();
                search.idClients.Add("0");
            }


          
             result = await _context.ProjetLivraisons
                .Include(e => e.DetailLivraisons)
                .Include(e => e.Client)
                .Where(e =>
                e.StatusId.Equals("Delivered")
                &&
               (
                ((search.startDate==null||



                e.DeliveryDate>=search.startDate )
                
                && (search.endDate == null || e.DeliveryDate<=search.endDate))
               && (search.idClients.Contains("0") || search.idClients.Contains(e.Client.Nom)) 
               && (( String.IsNullOrEmpty(search.type)|| search.type.Equals("deux") )|| (e.Nature.ToLower().Equals(search.type.ToLower()))))
               ).ToListAsync();
            CountTotal = result.Count();
            CountDone = result.Where(e => e.DeliveryDate <= (e.PlannedDate)).Count();
            try {


                result.ForEach(e =>
                {

                    if (e.DeliveryDate.HasValue) 
                    
                    {
                    if ((e.DeliveryDate-e.PlannedDate).Value.TotalDays>=7 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 14)
                        {

                            CountDate1Semaine++;
                        }
                       else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 14 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 28)
                        {

                            CountDate2Semaine++;
                        }
                        else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 28 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 35)
                        {

                            CountDate4Semaine++;
                        }
                        else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 35)
                        {

                            CountDate5Semaine++;
                        }

                    }


                });


            }
            catch {}
            var res = new { CountDone,CountTotal,CountDate1Semaine,CountDate2Semaine,CountDate4Semaine,CountDate5Semaine};


            //if (search != null)
            //{
            //    result = await _context.ProjetLivraisons.Where(e=>e.StartDate >=(search.startDate)|| e.DeliveryDate<=search.endDate ||e.Nature.Equals(search.nature)).ToListAsync(); }

            return Ok(res);
            
        }







        [HttpPost("LivraisonStatus2")]
        public async Task<IActionResult> ProjetLivraisonStatus2(SecondSearch search)
        {
            var CountTotal = 0;
            var CountDone = 0;
            var CountDate1Semaine = 0;
            var CountDate2Semaine = 0;
            var CountDate4Semaine = 0;
            var CountDate5Semaine = 0;

            List<ProjetLivraison> result = new List<ProjetLivraison>();
            if (search.idClients == null)
            {
                search.idClients = new List<string>();
                search.idClients.Add("0");
            }



            result = await _context.ProjetLivraisons
               .Include(e => e.DetailLivraisons)
               .Include(e => e.Client)
               .Where(e =>
               e.StatusId.Equals("Delivered")
               &&
              (
               ((search.startDate == null ||



               e.InitialPlannedDate >= search.startDate)

               && (search.endDate == null || e.InitialPlannedDate <= search.endDate))
              && (search.idClients.Contains("0") || search.idClients.Contains(e.Client.Nom))
              && ((String.IsNullOrEmpty(search.type) || search.type.Equals("deux")) || (e.Nature.ToLower().Equals(search.type.ToLower()))))
              ).ToListAsync();
            CountTotal = result.Count();
            //CountDone = result.Where(e => e.InitialPlannedDate <= (e.DeliveryDate)).Count();

            try  
            {


              result.ForEach(e => 
   
                {

                    if (e.DeliveryDate.HasValue )

                    {

                        double difference = (e.DeliveryDate - e.InitialPlannedDate).Value.TotalDays;

                        if(difference<7)
                        {

                            CountDone++;
                        }



                      else  if (difference<14 && difference>=7)
                        {

                            CountDate1Semaine++;
                            
                        }
                        else if (difference>=14 && difference<28)
                        {

                            CountDate2Semaine++;
                        }
                        else if (difference<35 && difference>=28)
                        {

                            CountDate4Semaine++;
                        }
                       else  if (difference >= 35)
                        {

                            CountDate5Semaine++;
                        }

                    }


                });


            }
            catch { }
            var res = new { CountDone, CountTotal, CountDate1Semaine, CountDate2Semaine, CountDate4Semaine, CountDate5Semaine };


            //if (search != null)
            //{
            //    result = await _context.ProjetLivraisons.Where(e=>e.StartDate >=(search.startDate)|| e.DeliveryDate<=search.endDate ||e.Nature.Equals(search.nature)).ToListAsync(); }

            return Ok(res);

        }




        [HttpPost("LivraisonStats")]
        public async Task<IActionResult> LivraisonStat()
        {
            var CountTotal = 0;
            var CountDone = 0;
            var CountDate1Semaine = 0;
            var CountDate2Semaine = 0;
            var CountDate4Semaine = 0;
            var CountDate5Semaine = 0;
            var result = new List<ProjetLivraison>();
            
                result = await _context.ProjetLivraisons.Include(e => e.DetailLivraisons).Where(e => e.StatusId.Equals("Delivered")).ToListAsync();
           

            CountTotal = result.Count();
            CountDone = result.Where(e => e.DeliveryDate <= (e.PlannedDate)).Count();
            try
            {


                result.ForEach(e =>
                {

                    if (e.DeliveryDate.HasValue)

                    {
                        if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 7 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 14)
                        {

                            CountDate1Semaine++;
                        }
                        else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 14 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 28)
                        {

                            CountDate2Semaine++;
                        }
                        else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 28 && (e.DeliveryDate - e.PlannedDate).Value.TotalDays < 35)
                        {

                            CountDate4Semaine++;
                        }
                        else if ((e.DeliveryDate - e.PlannedDate).Value.TotalDays >= 35)
                        {

                            CountDate5Semaine++;
                        }

                    }


                });


            }
            catch { }
            var res = new { CountDone, CountTotal, CountDate1Semaine, CountDate2Semaine, CountDate4Semaine, CountDate5Semaine };


            //if (search != null)
            //{
            //    result = await _context.ProjetLivraisons.Where(e=>e.StartDate >=(search.startDate)|| e.DeliveryDate<=search.endDate ||e.Nature.Equals(search.nature)).ToListAsync(); }

            return Ok(res);

        }

        [HttpPost("GetClientNames")]
        public async Task<IActionResult> GetClientNames()
        {
            List<string> names= new List<string>();
            
            var result =await  _context.Clients.ToListAsync();
            foreach( var a in result)
            {
                names.Add(a.Nom);
            }
            string[] noms = names.ToArray();
            var res =  noms;


            //if (search != null)
            //{
            //    result = await _context.ProjetLivraisons.Where(e=>e.StartDate >=(
            //
            //    .startDate)|| e.DeliveryDate<=search.endDate ||e.Nature.Equals(search.nature)).ToListAsync(); }

            return Ok(res);

        }












    }
}
