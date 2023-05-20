using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using ProjetBack.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Gateway.Dtos.Account;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using ProjetBack.Dtos;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ProjetsController : ControllerBase
    {
        private readonly PilotageDBContext _context;
        private readonly IConfiguration _config;
        private SqlConnection con;
        private readonly AppSettings _appSettings;

        public ProjetsController(IConfiguration config, PilotageDBContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings?.Value;

            _config = config;
            var x = _config["ConnectionString:PilotageDB"];
            con = new SqlConnection(_config["ConnectionString:PilotageDB"]);
            con.Open();
        }


        //[HttpGet("byManager/{idManager}")]
        //public object GetByManager(string idManager)
        //{
        //    var query = "select p.Nommenclature,  p.Id, p.DateCreation, p.DateDebut, p.Publique, p.PhaseProjetId, p.Description,p.DateFin,count(e.ProjetId) as equipeCount,count(c.ProjetId) as clientCount from Projets p join ProjetEquipes e  on p.Id = e.ProjetId JOIN    Equips t on e.EquipeId = t.Id join ClientProjet c on p.Id = c.ProjetId where t.ManagerId = '" + idManager + "' group by p.Nommenclature,  p.Id, p.DateCreation, p.DateDebut, p.Publique, p.PhaseProjetId, p.Description,p.DateFin";
        //    return ExecuteRequest(query);
        //}

        [NonAction]

        public DataTable ExecuteRequest(string requete)
        {
            //SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter sda;
            DataTable dt;




            dt = new DataTable("why");
            if (requete != string.Empty)
            {

                cmd = new SqlCommand(requete, con);
                cmd.CommandTimeout = 0;
                sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);


            }


            return dt;
        }

        [HttpPost("GetprojetStatClientType")]
        public async Task<ActionResult<User>> GetprojetStatClientType(Search searsh)
        {
            var resultatAll = (
                            from detail in _context.DetailImputations

                            join  imputation in _context.Imputations on detail.ImputationId equals imputation.Id
                          
                            join tache in _context.Taches on detail.TacheId equals tache.Id
                            join detailL in _context.DetailLivraisons on tache.detailLivraisonId equals detailL.Id
                            join projet in _context.Projets on detailL.ProjetId equals projet.Id
                            join livraison in _context.ProjetLivraisons on detailL.ProjetLivraisonId equals livraison.Id
                            join client in _context.Clients on livraison.ClientId equals client.Id
                            where 

                    (searsh.idClients.Count() == 0 || searsh.idClients.Contains(client.Id)) && 
             
                    (searsh.startDate == null || imputation.DateDebut >= searsh.startDate) && 
                    (searsh.endDate == null || imputation.DateFin <= searsh.endDate)
                            select new
                            {
                                detail,
                                projetName = projet.Nommenclature,
                                projetId = projet.Id,
                                projetType = projet.Type,

                                clientName = client.Nom,
                                clientId = client.Id,
                            }

                               ).ToList();
            var chargeConsomeTotale = resultatAll.Select(x => x.detail).Sum(y => GetTimeInHours(y));

            var stat =  (from r in resultatAll
             group r by r.projetType into g
             select new
             {
                 g.Key,
                 chargeParType = g.Select(x => x.detail).Sum(y => GetTimeInHours(y)),
                 nbTypes = g.GroupBy(x=>x.projetId).Count()
        
             }).ToList();

            var resultatbyFilterType = resultatAll.Where(x => searsh.types.Count() == 0 || searsh.types.Contains(x.projetType)).ToList();
            var groupByProjet = (from r in resultatbyFilterType
                                 group r by r.projetType into g
                         select new
                         {
                             g.Key,
                             chargeParType = g.Select(x => x.detail).Sum(y => GetTimeInHours(y)),
                             projets =
                                       (from i in g
                                        group i by (i.projetId, i.projetName) into g2
                                        select new
                                        {

                                            g2.Key.projetName,
                                            chargeParProjet = g2.Select(x => x.detail).Sum(y => GetTimeInHours(y)),
                                            Clients =
                                       (from i in g2
                                        group i by (i.clientId, i.clientName) into g3
                                        select new
                                        {
                                            g3.Key.clientName,
                                            chargeParClient = g3.Select(x => x.detail).Sum(y => GetTimeInHours(y)),


                                        }
                                        )

                                        }
                                        )

                         }).ToList();



            var groupbyClient = (from r in resultatbyFilterType
                                 group r by (r.clientId, r.clientName) into g
                         select new
                         {
                             g.Key.clientName,
                             chargeParClient = g.Select(x => x.detail).Sum(y => GetTimeInHours(y)),

                             types =
                                       (from i in g
                                        group i by i.projetType into g2

                                        select new
                                        {

                                            g2.Key,
                                            chargeParTypes = g2.Select(x => x.detail).Sum(y => GetTimeInHours(y)),
                                            
                                            projets =
                                       (from i in g2
                                        group i by (i.projetId, i.projetName) into g3

                                        select new
                                        {

                                            g3.Key.projetName,
                                            chargeParProjet = g3.Select(x => x.detail).Sum(y => GetTimeInHours(y)),
                                            g3.Key,



                                        }
                                        )



                                        }
                                        ),

                       
                         }).ToList();


            return Ok(new { chargeConsomeTotale, groupByProjet, groupbyClient, stat });


        }
        [NonAction]

        public double GetTimeInHours(DetailImputation detail)
        {
            TimeSpan ConvertToTimeSpan(string time)
            {
                if (string.IsNullOrWhiteSpace(time))
                    return new TimeSpan(0, 0, 0);
                var timecomponents = time.Split(':', StringSplitOptions.RemoveEmptyEntries);
                int.TryParse(timecomponents[0], out int hours);
                int.TryParse(timecomponents[1], out int minutes);

                return new TimeSpan(hours, minutes, 0) / 8;
            }
            if (detail == null) detail = new DetailImputation();

            return ConvertToTimeSpan(detail.Monday).Add(ConvertToTimeSpan(detail.Tuesday))
                      .Add(ConvertToTimeSpan(detail.Wednesday)).Add(ConvertToTimeSpan(detail.Thursday))
                      .Add(ConvertToTimeSpan(detail.Friday))
                      .TotalHours;

        }



        // GET: api/Projets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projet>>> GetnommenclaturProjets()
        {
            //var managersname = _context.Projets.SelectMany(x => x.projetsEquipe).Select(x => x.Equipe).Select(x => x.Manager).Select(x => x.FullName).ToList();


            var result = (from projet in _context.Projets
                          where projet.lockoutEnabled == true
                          select new
                          {

                             // managersname = projet.projetsEquipe.Select(x => x.Equipe).Select(x => x.Manager).GroupBy(x => x.FullName),


                              //    managersname = projet.projetsEquipe.Select(x => x.Equipe).Select(x => x.Manager).Distinct().Select(x => new { x.FullName }).ToList(),
                              Description = projet.Description,
                              Publique = projet.Publique,
                              DateCreation = projet.DateCreation,
                              Createur = projet.Createur,
                              DateDerniereModification = projet.DateDerniereModification,
                              PhaseProjetId = projet.PhaseProjetId,
                   
                              Id = projet.Id,
                              lockoutEnabled = projet.lockoutEnabled,
                              Nommenclature = projet.Nommenclature,
                              DateDebut = projet.DateDebut,
                              DateFin = projet.DateFin,
                              projet.Type,
                              phaseProjet = projet.PhaseProjetId,
                              ClientCount = projet.ClientProjets.Count(),
                              EquipeCount = projet.projetsEquipe.Count(),
                              DetailLivraisonsCount = projet.DetailLivraisons.Count(),

                          }).ToList();

            return Ok(result);

        }
        [HttpGet("byManager/{idManager}")]
        public async Task<ActionResult<IEnumerable<Projet>>> GetByManager(string idManager)
        {


            var result = (from projet in _context.Projets
                        //  join projetsEquipe in _context.ProjetEquipes on projet.Id equals projetsEquipe.ProjetId


                      //    join Equipe in _context.Equips on projetsEquipe.EquipeId equals Equipe.Id


                          where projet.lockoutEnabled == true
                          //&& Equipe.ManagerId == idManager
                          //group new { projet } by new { projet } into grp
                          //orderby grp.Key

                          select new
                          {
                              Description = projet.Description,
                              Publique = projet.Publique,
                              DateCreation = projet.DateCreation,
                              Createur = projet.Createur,
                              DateDerniereModification = projet.DateDerniereModification,
                              PhaseProjetId = projet.PhaseProjetId,

                              Id = projet.Id,
                              lockoutEnabled = projet.lockoutEnabled,
                              Nommenclature = projet.Nommenclature,
                              DateDebut = projet.DateDebut,
                              DateFin = projet.DateFin,

                              DetailLivraisonsCount = projet.DetailLivraisons.Count(),
                              phaseProjet = projet.PhaseProjetId,
                              ClientCount = projet.ClientProjets.Count(),
                              EquipeCount = projet.projetsEquipe.Count(),
                             // managersname = projet.projetsEquipe.Select(x => x.Equipe).Select(x => x.Manager.FullName).Distinct().ToList(),

                             // managersname = projet.projetsEquipe.Select(x => x.Equipe).Select(x => x.Manager).GroupBy(x => x.FullName),
                                       
                              isManager = projet.projetsEquipe.Select(x=>x.Equipe).Where(x=>x.ManagerId== idManager).Distinct().Count()
                          }
                         ).Distinct();

            return Ok(result);

        }
        //[HttpGet("withUsers")]
        //public  IActionResult GetProjectWithUser()
        //{


        //    var projet = _context.Projets.Where(x=>x.lockoutEnabled==true).Include(y => y.ProjetUsers).ToList();

        //    if (projet == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(projet);


        //}

        [HttpGet("GetProjectForReporting")]
        public IActionResult GetProjectForReporting()
        {
            var projets = (from projects in _context.Projets
                           join detailImputations in _context.DetailImputations on projects.Id equals detailImputations.IdProjet
                           join Imputation in _context.Imputations on detailImputations.ImputationId equals Imputation.Id
                           where Imputation.StatusImputation==3 && projects.lockoutEnabled==true
                           group new { projects } by new { projects.Id, projects.Nommenclature } into grp
                           select new 
                           {
                              Id = grp.Key.Id,
                              Nommenclature = grp.Key.Nommenclature,
                           }).ToList();

            return Ok(projets);
        }

        [HttpGet("byUser/{iduser}")]
        public IActionResult GetProjectByUser(string iduser)
        {
            //var projets= _context.Projets.Include(y => y.projetsEquipe).Where(x => x.lockoutEnabled == true).ToList();

            //var projets = (from projects in _context.Projets
            //               join ProjetUsers in _context.ProjetUsers on projects.Id equals ProjetUsers.ProjetId
            //               where ProjetUsers.UserId == iduser && projects.lockoutEnabled == true
            //               select new
            //               {
            //                   id = projects.Id,
            //                   Nommenclature = projects.Nommenclature,
            //                   ProjetUsers = projects.ProjetUsers

            //               }).ToList();



            var projets = (from projects in _context.Projets
                           join ProjetEquipes in _context.ProjetEquipes on projects.Id equals ProjetEquipes.ProjetId
                           join equipe in _context.Equips on ProjetEquipes.EquipeId equals equipe.Id
                         //  join users in _context.Users on equipe.Id equals users.EquipeId
                          // where users.IdUser == iduser && projects.lockoutEnabled == true
                           select new
                           {
                               id = projects.Id,
                               Nommenclature = projects.Nommenclature,
                              
                           }).ToList();

            return Ok(projets);
        }

        [HttpGet("GetProjectUsersByUser/{iduser}")]
        public IActionResult GetProjectUsersByUser(string iduser)
        {
            var userlist = (from ProjetUsers in _context.ProjetUsers
                            join users in _context.Users on ProjetUsers.UserId equals users.IdUser
                            where ProjetUsers.UserId == iduser 
                            select new
                            {
                                UserId = users.IdUser,
                                FullName = users.FullName,
                            }).ToList();

            return Ok(userlist);
        }
        [HttpGet("GetProjectUsersByManager/{iduser}")]
        public IActionResult GetProjectUsersByManager(string idmanager)
        {
            var userlist = (from ProjetUsers in _context.ProjetUsers
                            join users in _context.Users on ProjetUsers.UserId equals users.IdUser
                          //  where users.Valideur1Id == idmanager || users.Valideur2Id == idmanager || users.IdUser == idmanager 
                            select new
                            {
                                UserId = users.IdUser,
                                FullName = users.FullName,
                            }).ToList();

            return Ok(userlist);
        }

        [HttpGet("GetProjectUsers")]
        public IActionResult GetProjectUsers()
        {
            var userlist = (from ProjetUsers in _context.ProjetUsers
                           join users in _context.Users on ProjetUsers.UserId equals users.IdUser
                            select new
                           {
                               UserId= users.IdUser,
                               FullName = users.FullName,
                           }).ToList();

            return Ok(userlist);
        }
        // GET: api/Projets/5
        [HttpGet("{id}")]
        public IActionResult GetProjet(string id)
        {
            var projet = _context.Projets.Where(x => x.Id == id && x.lockoutEnabled == true).Include(x=>x.projetsEquipe ).Include(x => x.ClientProjets).FirstOrDefault();

            if (projet == null)
            {
                return NotFound();
            }

            return Ok(projet);
        }


        [HttpGet("GetProjetWithClientProject")]
        public IActionResult GetProjetWithClientProject()
        {
            var projet = _context.Projets.Where(x =>  x.lockoutEnabled == true).Include(x => x.ClientProjets).ToList();

            if (projet == null)
            {
                return NotFound();
            }

            return Ok(projet);
        }

        [HttpGet("GetProjetWithClientProjectbyEquipe/{EquipeId}")]
        public IActionResult GetProjetWithClientProject(string EquipeId)
        {
            var projet = (from project in _context.Projets
                          join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                          where ProjetEquipes.EquipeId == EquipeId && project.lockoutEnabled==true
                          select new
                          {

                              project.Id,
                              project.Nommenclature,
                              project.ClientProjets
                          }).ToList();


            if (projet == null)
            {
                return NotFound();
            }

            return Ok(projet);
        }


        [HttpGet("GetProjetWithClientProjectbyUser/{idUser}")]
        public IActionResult GetProjetWithClientProjectbyuser(string idUser)
        {
            var projet = (from project in _context.Projets
                          join ProjetEquipes in _context.ProjetEquipes on project.Id equals ProjetEquipes.ProjetId
                          join userEquipe in _context.EquipeUser on ProjetEquipes.EquipeId equals userEquipe.EquipeId
                          where userEquipe.UserId== idUser && project.lockoutEnabled == true
                          select new
                          {

                              project.Id,
                              project.Nommenclature,
                              project.ClientProjets
                          }).Distinct().ToList();


            if (projet == null)
            {
                return NotFound();
            }

            return Ok(projet);
        }


        [HttpGet("GetProjetWithClientProjectByManager/{idManager}")]
        public IActionResult GetProjetWithClientProjectByManager(string idManager)
        {
            var projets = (from projet in _context.Projets
                          join projetsEquipe in _context.ProjetEquipes on projet.Id equals projetsEquipe.ProjetId
                          join Equipe in _context.Equips on projetsEquipe.EquipeId equals Equipe.Id
                           join userEquipe in _context.EquipeUser on Equipe.Id equals userEquipe.EquipeId

                           where (projetsEquipe.Equipe.ManagerId== idManager || userEquipe.UserId == idManager) && projet.lockoutEnabled
                           //  group new { projets, ClientProjets } by new { projets.Id, projets.Nommenclature, ClientProjets } into grp


                           select new
                          {

                               projet.Id,
                               projet.Nommenclature,
                               projet.ClientProjets


                               //grp.Key.Id,
                               //grp.Key.Nommenclature,
                               //grp.Key.ClientProjets
                           }).Distinct().ToList();




            // var projet =_context.Projets.Where(x => x.lockoutEnabled == true && x.projetsEquipe.Where(y => y.Equipe.ManagerId == idManager)).Include(x => x.projetsEquipe.Where(y=>y.Equipe.ManagerId== idManager)).Include(x => x.ClientProjets).ToList();

            //if (projet == null)
            //{
            //    return NotFound();
            //}

            return Ok(projets);
        }


        // GET: api/Projets/5
        //[HttpGet("GetProjetByIdUser/{iduser}")]
        //public  ActionResult GetProjetByIdUser(string idUser)
        //{



        //    var projet = (
        //                 from projets in _context.Projets
        //                 join tache in _context.Taches on projets.Id  equals tache.ProjetId
        //                 where tache.UserId == idUser && projets.lockoutEnabled == true
        //                 group new { projets } by new { projets.Id, projets.Nommenclature, projets.IdClient  }
        //            into grp
        //                 select new
        //                  {

        //                      Id = grp.Key.Id,
        //                      Nommenclature = grp.Key.Nommenclature,
        //                      IdClient = grp.Key.IdClient,

        //                  }).ToList();

        //    if (projet == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(projet);
        //}
        //public async Task<IActionResult> GetClientProjects(string clientId)
        //{
        //    _context.Projets.Where(x=>x.cli)
        //}

        //// GET: api/Projets/client

        ////[HttpGet("client/{id}")]
        ////public async Task<ActionResult> GetProjetClient(string id)
        ////{
        ////    _context.ProjetLivraisonClients.Where(x => x.ClientId == id).Select(x => x.ProjetLivraison.)
        ////    var result = await _context.Projets.Where(x => x.DetailLivraison.ProjetLivraison.ProjetLivraisonClients.cli == ClientId).ToListAsync();
        ////    return Ok(result);
        ////}




        // PUT: api/Projets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("Update/{id}")]
        //public async Task<IActionResult> PutProjet(string id, Projet projet)
        //{
        //    if (id != projet.IdProjet)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(projet).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProjetExists(id))
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

        // POST: api/Projets
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.


        [HttpPost]
        public async Task<ActionResult<Projet>> PostProjet(Projet projet)
        {
            var isNew = false;
            if (string.IsNullOrEmpty(projet.Id))
            {
                isNew = true;
                projet.DateCreation = DateTime.Now.ToString();
                _context.Projets.Add(projet);

                foreach (var item in projet.ClientProjets)
                {
                    item.ProjetId = projet.Id;

                    _context.ClientProjet.Add(item);
                }

             
       
            }
            else
            {


                var detail = _context.ClientProjet.Where(x => x.ProjetId == projet.Id).ToList();
                var equips = _context.ProjetEquipes.Where(x => x.ProjetId == projet.Id).ToList();
                _context.ClientProjet.RemoveRange(detail);
                await _context.SaveChangesAsync();
                _context.ClientProjet.AddRange(projet.ClientProjets);
                _context.ProjetEquipes.RemoveRange(equips);

                _context.ProjetEquipes.AddRange(projet.projetsEquipe);
                _context.Entry(projet).State = EntityState.Modified;
                await _context.SaveChangesAsync();





                //foreach (var item in detail)
                //{
                //    // if(item.ProjetLivraisonId==null)
                //    _context.ClientProjet.Remove(item);
                //}

                //foreach (var item in projet.ClientProjets)
                //{
                //   // if (item.ProjetLivraisonId == null) 
                //       _context.ClientProjet.Add(item);

                //}

            }
            try
            {
             

                await _context.SaveChangesAsync();
                if (isNew)
                {

                    foreach (ProjetEquipe item in projet.projetsEquipe)
                    {
                        var users = (from user in _context.Users
                                     join equipeusers in _context.EquipeUser on user.IdUser equals equipeusers.UserId
                                     where equipeusers.EquipeId == item.EquipeId
                                     select user).Distinct().ToList();
                        foreach (var user in users)
                        {

                            var message = "<div style=''>Bonjour <strong>" + user.Nom + " " + user.Prenom + "</strong>," + " <p> Bienvenue sur la plateforme System info , vous êtes affectés  au projet <strong>" + projet.Nommenclature + "</strong> </p> <p> Vous pouvez maintenant l'ajouter comme un detail de projet livraison.</p>" + " <p><a href='http://192.168.1.40/PortailSysInfo'> Cliquez ici pour vous connecter </a>.</p></div>";
                            var subject = "Projet " + projet.Nommenclature;
                            SendEmail(user.AdresseEmail, message, subject);
                        }


                    }
                }
                return Ok();

            }
            catch
            {
                throw;
            }

        }
        [NonAction]
        private void SendEmail(string email, string messageBody, string subject)
        {
            SmtpClient client = new SmtpClient(_appSettings.Host, _appSettings.EmailPort);

            client.EnableSsl = _appSettings.EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_appSettings.EmailSender, _appSettings.Password);
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            mailMessage.From = new MailAddress(_appSettings.EmailSender);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = messageBody;
            client.Send(mailMessage);

        }

        [NonAction]
        public Task SendAsync(string email, string messageBody, string subject)
        {
            var _sender = _appSettings.EmailSender;
            var _user = _appSettings.EmailUser;
            var _password = _appSettings.Password;
            var _smtpClient = _appSettings.Host;
            int _port = _appSettings.EmailPort;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_smtpClient);
            mail.From = new MailAddress(_sender);

            mail.To.Add(email);

            mail.Subject = subject;
            mail.Body = messageBody;

            SmtpServer.Port = _port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(_user, _password);
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch { }
            return Task.FromResult(0);
        }

        // DELETE: api/Projets/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Projet>> DeleteProjet(string id)
        {
            var projet = await _context.Projets.FindAsync(id);
            if (projet == null)
            {
                return NotFound();
            }

            _context.Projets.Remove(projet);
            await _context.SaveChangesAsync();

            return projet;
        }
        [HttpPost("Update/{id}")]
        public async Task<ActionResult<Projet>> put(string id, Projet projet)
        {
   
            if (projet.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(projet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                
                    throw;
                
            }

            return NoContent();
        }
        [NonAction]
        private bool ProjetExists(string id)
        {
            return _context.Projets.Any(e => e.Id == id);
        }
    }
}
