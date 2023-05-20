using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using ProjetBack.PushServices;

using ProjetBack.Models;

using System.Data;

using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Gateway.Dtos.Account;
using ProjetBack.Dtos;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class ImputationsController : ControllerBase
    {
        private readonly PilotageDBContext _context;
        private readonly IHubContext<NotificationHub, INotificationHubService> _hubContext;
        string monday;
        private readonly AppSettings _appSettings;

        public ImputationsController(PilotageDBContext context, IHubContext<NotificationHub, INotificationHubService> hubContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;

            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Imputations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imputation>>> GetImputations()
        {
            return await _context.Imputations.ToListAsync();
        }

        // GET: api/Imputations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Imputation>> GetImputation(string id)
        {
            var imputation = await _context.Imputations.FindAsync(id);

            if (imputation == null)
            {
                return NotFound();
            }

            return imputation;
        }
        //get special imputation by date : api/Imputations/04-May-2020/08-May-2020
        [HttpGet("{startDate}/{endDate}")]
        public async Task<ActionResult<Imputation>> GetImputationByDates(string startDate, string endDate)
        {
            
            var imputation = await _context.Imputations.FirstOrDefaultAsync(
                x => x.DateFin.Date == Convert.ToDateTime(endDate).Date && x.DateDebut.Date == Convert.ToDateTime(startDate));

            if (imputation == null)
            {
                return new Imputation();
            }

            return imputation;
        }

        //*******utiliser cette modification***//
        //get special imputation for each special user by date : api/Imputations/319cadae-9e67-4ee5-995f-42f6a7487bda/04-May-2020/08-May-2020
        [HttpGet("{idUser}/{startDate}/{endDate}")]
        public async Task<ActionResult<Imputation>> GetImputationUserByDates(string startDate, string endDate,string idUser)
        {

            var imputation = await _context.Imputations.FirstOrDefaultAsync(
                x => x.DateFin.Date == Convert.ToDateTime(endDate).Date && x.DateDebut.Date == Convert.ToDateTime(startDate) && x.UserId == idUser);

            if (imputation == null)
            {
                var imp = new Imputation();
                imp.StatusImputation =1;
                return imp;
            }

            return imputation;
        }






        //get details in list  imputation (give id for imputation):api/Imputations/details/02e5f55a-f0da-4b89-a939-0a6d8dca0ac8
        #region Details
        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetDetailImputation(string id)
        {
            var _result = from detailImputations in _context.DetailImputations
                          join tache in _context.Taches on detailImputations.TacheId equals tache.Id
                          join detailLivraisons in _context.DetailLivraisons on tache.detailLivraisonId equals detailLivraisons.Id

                          where detailImputations.ImputationId == id select new
                          {

                              NomProjetLivraison = detailLivraisons.ProjetLivraison.ProjetName +" V"+ detailLivraisons.ProjetLivraison.Delivery,
                              idProjetLivraison = detailLivraisons.ProjetLivraisonId ,
                              NomDetail = detailLivraisons.Projet.Nommenclature + " V" + detailLivraisons.Delivery,
                              idDetail= detailLivraisons.Id,
                              Nomtache = tache.Description,
                             // NomProject = project.Nommenclature,

                              Id= detailImputations.Id,
                              Monday = detailImputations.Monday,
                              Tuesday = detailImputations.Tuesday,
                              Wednesday = detailImputations.Wednesday,
                              Thursday = detailImputations.Thursday,
                              Friday = detailImputations.Friday,
                        
                        
                        
                        
                          };
            var listResult = await _result.ToListAsync();

            return Ok(listResult);
        
        }



        //get details of imputation for each user  (give id for imputation et id User):api/Imputations/details/idImputation/idUser

        [HttpGet("details/{id}/{idUser}")]
        public async Task<ActionResult> GetDetailImputationForEachUser(string id, string idUser)
        {
            var result = await _context.DetailImputations.Include(x => x.tache).ThenInclude(x => x.detailLivraison).ThenInclude(x => x.ProjetLivraison).
                Where(x => x.ImputationId == id && x.Imputation.UserId == idUser).Select(x => new
            {
                x.Id,
                x.Monday,
                x.Tuesday,
                x.Wednesday,
                x.Thursday,
                x.Friday,
                x.TacheId,
                x.IdProjet,
                x.IdClient,

                projetLivraisonId = x.tache.detailLivraison.ProjetLivraisonId,
                detailLivraisonId = x.tache.detailLivraison.Id,
                tacheName=x.tache.Description,
                detailLivraisonName = x.tache.detailLivraison.Projet.Nommenclature + " V" + x.tache.detailLivraison.Delivery,
                projetLivraisonName = x.tache.detailLivraison.ProjetLivraison.ProjetName + " V" + x.tache.detailLivraison.ProjetLivraison.Delivery,
                //  Projet = x.Imputation.User.Taches.FirstOrDefault().Projet.Nommenclature,
                //  Tache = x.Imputation.User.Taches.FirstOrDefault().Description,

                //  StatusImputation = x.Imputation.StatusImputation,
                // Client = x.Imputation.User.Taches.FirstOrDefault().Projet.DetailLivraisons.FirstOrDefault().ProjetLivraison.ProjetLivraisonClients.FirstOrDefault().Client.Nom

            }).ToListAsync();

            return Ok(result);
        }

        [HttpPost("getStatDateValidationImputation")]
        public async Task<ActionResult> ChangeStatus(Search searsh)
        {
            try
            {
                var imputation = await _context.Imputations.Where(x => x.UserId == searsh.idUser && (searsh.endDate==null || x.DateFin.Date <= searsh.endDate.Value.Date) && (searsh.startDate == null || x.DateDebut.Date>=searsh.startDate.Value.Date) && x.DateAvalide != null).Select(x => new { x.DateAvalide, x.DateFin }).ToListAsync();
                
                if (imputation.Count > 0) { 
                var late = imputation.Where(x => (x.DateAvalide.Value - x.DateFin).TotalDays >= 4).Count();
                var onTime = imputation.Where(x => (x.DateAvalide.Value - x.DateFin).TotalDays < 4).Count();
                return Ok(new { late, onTime });
                }
                else return Ok(new { late=0, onTime=0 });
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
      
        }




        [HttpPost("changeStatus/{imputationId}/{statusId}")]
        public async Task<ActionResult> ChangeStatus(string imputationId, int StatusImputation)
        {
            var imputation = _context.Imputations.FirstOrDefault(x => x.Id == imputationId);

            if (imputation == null)
                return NotFound();

            imputation.StatusImputation = StatusImputation;
            await _context.SaveChangesAsync();

            return Ok();
        }




        // PUT: api/Imputations/ChangeStatus

        [HttpPost("Update/changeStatusValide/{imputationId}/{statusId}")]
        public async Task<IActionResult> ChangeStatusValide(string imputationId, int statusId)
        {
            var imputation = _context.Imputations.FirstOrDefault(x => x.Id == imputationId);
            if (imputation == null)
                return NotFound();

            imputation.StatusImputation = statusId;


            _context.Entry(imputation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                if (imputation.StatusImputation == 1) {
                var user =  _context.Users.FirstOrDefault(x => x.IdUser == imputation.UserId);

                var message = "<div style=''>Bonjour <strong>" + user.Nom + " " + user.Prenom + "</strong>," + " <p> Bienvenue sur la plateforme System info , Merci de verifier l'imputation du  <strong>" + imputation.DateDebut +" -> "+ imputation.DateFin + "</strong> </p> </div>";
                var subject = "Imputation " +imputation.DateDebut + " -> " + imputation.DateFin ;
                SendEmail(user.AdresseEmail, message, subject);
                }

                return Ok();

          
            }
            catch (Exception e)
            {
                return BadRequest(e);

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





        [HttpPost("details")]
        public async Task<ActionResult<DetailImputation>> PostDetailImputation(List<DetailImputation> detailImputation, bool extra, string id)
        {
               _context.DetailImputations.AddRange(detailImputation);

            //string userId = User.Claims.First(c => c.Type == "nameidentifier").Value;
            //if (string.IsNullOrEmpty(detailImputation[0].Id))
            //{
            //    // add
            //}
            //else
            //{
            //    // edit
            //    _context.Entry(detailImputation).State = EntityState.Modified;
        //}
            try
            {
                await _context.SaveChangesAsync();
               // ChangeStatusValide(_context.Imputations.First().Id, _context.Imputations.First().StatusImputationId);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            // when timesheet is submitted by engineer , then i need to notify managers or admins
            // so I select group and send notifications
            if (extra == true)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.IdUser == id);
                await _hubContext.Clients.Group(RoleType.Manager).GetNotifications(new Models.Notification { Title = user.FullName, Content = "Imputation", ActionLink = "/timesheet/" + "khaoula" });
                await _hubContext.Clients.Group(RoleType.Admin).GetNotifications(new Models.Notification { Title = user.FullName, Content = "Imputation", ActionLink = "/timesheet/" + "khaoula" });
            }
            return Ok();
        }
        #endregion


        [HttpGet("management/{startDate}/{endDate}/{managerId}")]
        public async Task<IActionResult> GetImputationForManager(string startDate, string endDate, string managerId)
        {


            var result = (from imp in _context.Imputations
                          join usersEquipe in _context.EquipeUser on imp.UserId equals usersEquipe.UserId
                          where usersEquipe.User.FirstImputation<= Convert.ToDateTime(endDate)

                          && (usersEquipe.User.LastImputation==null) &&
                          imp.StatusImputation != 1 && imp.DateFin == Convert.ToDateTime(endDate).Date && imp.DateDebut.Date == Convert.ToDateTime(startDate) && usersEquipe.Equipe.ManagerId == managerId
                          select new
                          {
                              Id = imp.Id,
                            imp.DateAvalide,
                              StatusImputation = imp.StatusImputation,
                              User = imp.User.Nom + "  " + imp.User.Prenom,
                          }).Distinct().ToList();

            var nbEmployeeTotale = (from user in _context.Users

                                    join eq in _context.EquipeUser on user.IdUser equals eq.UserId
                                    where eq.Equipe.ManagerId == managerId 
            && (user.FirstImputation == null || user.FirstImputation <= Convert.ToDateTime(endDate))

                          && (user.LastImputation >= Convert.ToDateTime(endDate) || user.LastImputation == null)
                                    select user.IdUser 
                                    
                                    ).Distinct().Count();

            var nbEmployeeImpute = result.Count();
            var nbEmployeeWithNoImputation= nbEmployeeTotale - nbEmployeeImpute;
            return Ok(new { result , nbEmployeeTotale , nbEmployeeWithNoImputation, nbEmployeeImpute });
        }

        [HttpGet("UsersWithNoImputationManager/{startDate}/{endDate}/{managerId}")]
        public async Task<IActionResult> GetUsersWithNoImputationManager(string startDate, string endDate, string managerId)
        {
            var result = (from imp in _context.Imputations
                          join usersEquipe in _context.EquipeUser on imp.UserId equals usersEquipe.UserId
                          where imp.StatusImputation != 1 && imp.DateFin.Date == Convert.ToDateTime(endDate).Date && imp.DateDebut.Date == Convert.ToDateTime(startDate) && usersEquipe.Equipe.ManagerId == managerId
                          select imp.UserId).Distinct().ToList();


            var result2 = (from u in _context.Users
                          join usersEquipe in _context.EquipeUser on u.IdUser equals usersEquipe.UserId
                          where  usersEquipe.Equipe.ManagerId == managerId && !result.Contains(u.IdUser)
            && (u.FirstImputation == null || u.FirstImputation <= Convert.ToDateTime(endDate))
           && (u.LastImputation >= Convert.ToDateTime(endDate) || u.LastImputation == null)
                           select u.FullName).Distinct().ToList();

            //var result1 = await _context.Users.Where(x => !result.Contains(x.IdUser) && (x.Type == UserType.User || x.Type == UserType.Manager)).Select(x => x.FullName).ToListAsync();

            return Ok(result2 );

        }

        #region Manager
        [HttpGet("management/{startDate}/{endDate}")]
        public async Task<IActionResult> GetImputationForManagerByManager(string startDate, string endDate)
        {

            try
            {

           
            
            var result = await _context.Imputations.Where(x => x.StatusImputation != 1 && x.DateFin.Date == Convert.ToDateTime(endDate).Date && x.DateDebut.Date == Convert.ToDateTime(startDate)).Include(x => x.User).Select(x => new
            {
                Id = x.Id,
                StatusImputation = x.StatusImputation,
                x.DateAvalide,

                User = x.User.Nom + "  " + x.User.Prenom,
                //Imputation = x.DateDebut + " - " + x.DateFin
            }).OrderBy(e => e.StatusImputation).ToListAsync();
            var nbEmployeeTotale = _context.Users.Where(x => 
            x.Type == UserType.Manager
            || x.Type == UserType.User 
            && (x.FirstImputation==null|| x.FirstImputation <= Convert.ToDateTime(endDate) )
           && (x.LastImputation >= Convert.ToDateTime(endDate) || x.LastImputation == null)
           ).Count();
            var nbEmployeeImpute = result.Count();
            var nbEmployeeWithNoImputation = nbEmployeeTotale - nbEmployeeImpute;
                return Ok(new { result, nbEmployeeTotale, nbEmployeeWithNoImputation, nbEmployeeImpute });
            }
            catch (Exception e )
            {
                Console.WriteLine("exception " + e.Message);
                return BadRequest();
            }
            //var result = await _context.Imputations.Where(x => x.StatusImputation != 1 && x.DateFin == endDate && x.DateDebut == startDate && x.User.Equipe.ManagerId == managerId).Include(x => x.User).ThenInclude(x => x.Equipe).Select(x => new
            //{
            //    Id = x.Id,
            //    StatusImputation = x.StatusImputation,
            //    User = x.User.Nom + "  " + x.User.Prenom,
            //    Imputation = x.DateDebut + " - " + x.DateFin
            //}).ToListAsync();
          
        }



        [HttpGet("UserWithNoImputation/{startDate}/{endDate}")]
        public async Task<IActionResult> UserWithNoImputation(string startDate, string endDate)
        {
            var result = await _context.Imputations.Where(x => x.StatusImputation != 1 && x.DateFin.Date == Convert.ToDateTime(endDate).Date && x.DateDebut.Date == Convert.ToDateTime(startDate).Date).Include(x => x.User).Select(x=>x.UserId).ToListAsync();

            var result1 = await _context.Users.Where(x => !result.Contains(x.IdUser) && (x.Type== UserType.User || x.Type== UserType.Manager)

            && (x.FirstImputation == null || x.FirstImputation <= Convert.ToDateTime(endDate))

                          && (x.LastImputation >= Convert.ToDateTime(endDate) || x.LastImputation == null)
            ).Select(x=>x.FullName).ToListAsync();

            return Ok( result1 );
        }
        #endregion


        #region ManagerValidate
        [HttpGet("managementValidate/{id}")]
        public async Task<IActionResult> GetImputationForManagerValidate(int StatusImputation)
        {
            var result = await _context.Imputations.Include(x => x.User).Where(x => x.StatusImputation == StatusImputation).Select(x => new
            {
                Id = x.Id,
                StatusImputation = x.StatusImputation,
                User = x.User.Nom + " " + x.User.Prenom,
                Imputation = x.DateDebut + " - " + x.DateFin
            }).ToListAsync();
            return Ok(result);
        }

        #endregion


        // PUT: api/Imputations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutImputation(string id, Imputation imputation)
        {
            if (id != imputation.Id)
            {
                return BadRequest();
            }

            _context.Entry(imputation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImputationExists(id))
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

        [HttpPost("VerifierChargeConsommé")]
        public async Task<ActionResult<Tache>> VerifierChargeConsommé(Imputation imputation)
        {

            List<Tache> list = new List<Tache>();

            foreach (var item in imputation.DetailImputations)
            {
                var tachhe = _context.Taches.Find(item.TacheId);

                List<DetailImputation> DetailImp = _context.DetailImputations.Where(x => x.TacheId == tachhe.Id).ToList();
                tachhe.chargeConsomme = "00.00";
                if (item.Id == null)
                    DetailImp.Add(item);

                if (!tachhe.publique)
                {
                    foreach (var detailI in DetailImp)
                    {
                        DetailImputation detail = new DetailImputation();
                        if (detailI.Id == item.Id)
                            detail = item;
                        else detail = detailI;

                        Double munits = (Int32.Parse(detail.Monday.Substring(3, 2)) + Int32.Parse(detail.Tuesday.Substring(3, 2)) + Int32.Parse(detail.Thursday.Substring(3, 2)) + Int32.Parse(detail.Wednesday.Substring(3, 2)) + Int32.Parse(detail.Friday.Substring(3, 2)));

                        Double hours = (Int32.Parse(detail.Monday.Substring(0, 2)) + Int32.Parse(detail.Tuesday.Substring(0, 2)) + Int32.Parse(detail.Thursday.Substring(0, 2)) + Int32.Parse(detail.Wednesday.Substring(0, 2)) + Int32.Parse(detail.Friday.Substring(0, 2)));


                        if (munits != 0)
                        {
                            Double charge = (munits / 60 + hours) / 8;
                            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                        }
                        else
                        {
                            Double charge = hours / 8;
                            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                        }


                    } }
            
                //   _context.Entry(tachhe).State = EntityState.Modified;
                list.Add(tachhe);


            }
        

            try
            {

                //await _context.SaveChangesAsync();
                //foreach (var item in imputation.DetailImputations)
                //{
                //    var tachhe = _context.Taches.Find(item.TacheId);
                //}

                return Ok(list);


            }
            catch (Exception e)
            {
                return BadRequest(e);
            }






            //List<Tache> list = new List<Tache>();

            //foreach (var item in imputation.DetailImputations)
            //{
            //    var tachhe = _context.Taches.Find(item.TacheId);
            //    //if (tachhe.premierImputation == null)
            //    //    tachhe.premierImputation = DateTime.Now;

            //    //tachhe.status = "2";


            //    var DetailImp = _context.DetailImputations.Where(x => x.TacheId == tachhe.Id).ToList();
            //    tachhe.chargeConsomme = "00.00";

            //    foreach (var detail in DetailImp)
            //    {
            //        Double munits = (Int32.Parse(detail.Monday.Substring(3, 2)) + Int32.Parse(detail.Tuesday.Substring(3, 2)) + Int32.Parse(detail.Thursday.Substring(3, 2)) + Int32.Parse(detail.Wednesday.Substring(3, 2)) + Int32.Parse(detail.Friday.Substring(3, 2)));

            //        Double hours = (Int32.Parse(detail.Monday.Substring(0, 2)) + Int32.Parse(detail.Tuesday.Substring(0, 2)) + Int32.Parse(detail.Thursday.Substring(0, 2)) + Int32.Parse(detail.Wednesday.Substring(0, 2)) + Int32.Parse(detail.Friday.Substring(0, 2)));


            //        if (munits != 0)
            //        {
            //            Double charge = (munits / 60 + hours) / 8;
            //            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
            //        }
            //        else
            //        {
            //            Double charge = hours / 8;
            //            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
            //        }


            //    }
    

            //_context.Entry(tachhe).State = EntityState.Modified;


            //}

            //try
            //{

            //   await _context.SaveChangesAsync();
            //    foreach (var item in imputation.DetailImputations)
            //    {
            //        var tachhe = _context.Taches.Find(item.TacheId);
            //        list.Add(tachhe);
            //    }

            //       return Ok(list.Distinct());


            //}
            //catch (Exception e)
            //{
            //    return BadRequest(e);
            //}

        }

        [NonAction]
        public async Task<ActionResult<string>> calculateChargeConsommé(Imputation imputation)
        {


           
            foreach (var item in imputation.DetailImputations)
            {
                var tachhe = _context.Taches.Find(item.TacheId);
                if (tachhe.premierImputation == null)
                    tachhe.premierImputation = DateTime.Now;

                tachhe.status = "2";


                var DetailImp = _context.DetailImputations.Where(x => x.TacheId == tachhe.Id).ToList();
                tachhe.chargeConsomme = "00.00";

                foreach (var detail in DetailImp)
                {


                    Double munits = (Int32.Parse(detail.Monday.Substring(3, 2)) + Int32.Parse(detail.Tuesday.Substring(3, 2)) + Int32.Parse(detail.Thursday.Substring(3, 2)) + Int32.Parse(detail.Wednesday.Substring(3, 2)) + Int32.Parse(detail.Friday.Substring(3, 2)));

                    Double hours = (Int32.Parse(detail.Monday.Substring(0, 2)) + Int32.Parse(detail.Tuesday.Substring(0, 2)) + Int32.Parse(detail.Thursday.Substring(0, 2)) + Int32.Parse(detail.Wednesday.Substring(0, 2)) + Int32.Parse(detail.Friday.Substring(0, 2)));


                    if (munits != 0)
                    {
                        Double charge = (munits / 60 + hours) / 8;
                        tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                    }
                    else
                    {
                        Double charge = hours / 8;
                        tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                    }

                 
                }
                _context.Entry(tachhe).State = EntityState.Modified;


            }

            try
            {

               await   _context.SaveChangesAsync();
                return "ok";


            }
            catch (Exception e)
            {
                return "error save charge"+ e;
            }

        }


        
        


        // POST: api/Imputations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Imputation>> PostImputation(ImputationDto model)
        {
            var imp = _context.Imputations.Where(x => x.DateDebut == model.imputation.DateDebut  && x.DateFin == model.imputation.DateFin && x.UserId == model.imputation.UserId  && (x.StatusImputation == 1|| x.StatusImputation == 2)).FirstOrDefault();
            if (model.imputation.StatusImputation == 2 && model.imputation.DateAvalide == null)
            {
                model.imputation.DateAvalide = DateTime.Now;
            }


            if (model.imputation.mondayDate == null)
            {

            }
            if (imp == null)
            {
                model.imputation.DateCreation = DateTime.Now.ToString();
                model.imputation.mondayDate = model.imputation.DateDebut;
                model.imputation.TuesdayDate = model.imputation.DateDebut.AddDays(1).Date;
                model.imputation.WednesdayDate = model.imputation.DateDebut.AddDays(2).Date;
                model.imputation.FridayDate = model.imputation.DateDebut.AddDays(4).Date;
                model.imputation.ThursdayDate = model.imputation.DateDebut.AddDays(3).Date;
       


                _context.Imputations.Add(model.imputation);

            }
            else
            {

                var DI= _context.DetailImputations.Where(x=>x.ImputationId==imp.Id);
               _context.DetailImputations.RemoveRange(DI);
                await _context.SaveChangesAsync();
                _context.Imputations.Remove(imp);
                await _context.SaveChangesAsync();
                model.imputation.DateCreation = imp.DateCreation;
                model.imputation.mondayDate = imp.mondayDate;
                model.imputation.TuesdayDate = imp.TuesdayDate;
                model.imputation.WednesdayDate = imp.WednesdayDate;
                model.imputation.ThursdayDate = imp.ThursdayDate;
                model.imputation.FridayDate = imp.FridayDate;

              _context.Imputations.Add(model.imputation);
            }
            try
            {

                await _context.SaveChangesAsync();
                await calculateChargeConsommé(model.imputation);
                if (model.deletedTask.Count() > 0)
                {
                    await calculateCahrgeConsomerForDeletedTask(model.deletedTask);
                }

                if (model.imputation.StatusImputation == 2) { 
                var user =  _context.Users.Where(x => x.IdUser == model.imputation.UserId).FirstOrDefault();

             
                    var teamsManagers = (from equipe in _context.Equips
                                join equipeUser in _context.EquipeUser on equipe.Id equals equipeUser.EquipeId
                                where equipeUser.UserId == model.imputation.UserId
                                group new { equipe } by new { equipe.ManagerId} into grp

                                select grp.Key.ManagerId
                                
                             ).ToList(); 

                   await _hubContext.Clients.Users(teamsManagers).GetNotifications(new Models.Notification { Title = user.FullName + "a ajouté une Imputation ", Content = model.imputation.DateDebut.Date.ToString() + "/" + model.imputation.DateFin.Date.ToString(), ActionLink = "/liste_imputation;startDate=" + model.imputation.DateDebut.Date.ToString() + "; endDate=" + model.imputation.DateFin.Date.ToString(), endDate = model.imputation.DateFin.Date.ToString(), startDate = model.imputation.DateFin.Date.ToString() });
                   await _hubContext.Clients.Group(RoleType.Admin).GetNotifications(new Models.Notification { Title = user.FullName + "a ajouté une Imputation ", Content = model.imputation.DateDebut.Date.ToString() + "/" + model.imputation.DateFin.Date.ToString(), ActionLink = "/liste_imputation;startDate=" + model.imputation.DateDebut.Date.ToString() + "; endDate=" + model.imputation.DateFin.Date.ToString(), endDate = model.imputation.DateFin.Date.ToString(), startDate = model.imputation.DateFin.Date.ToString() });

                }
            }
            catch (Exception e){
                return BadRequest(e);
            }

            return Ok(new {result= model.imputation.Id } );
        }
     
        [NonAction]
        public async Task<ActionResult<string>>  calculateCahrgeConsomerForDeletedTask(List<string> deletedTask)
        {
            foreach (var item in deletedTask)
            {
                var tachhe = _context.Taches.Find(item);
            

                var DetailImp = _context.DetailImputations.Where(x => x.TacheId == tachhe.Id).ToList();

                tachhe.chargeConsomme = "00.00";

                if (DetailImp.Count() == 0)
                {
                    tachhe.status = "1";
                    tachhe.premierImputation = null;
                }
                else
                {




                    foreach (var detail in DetailImp)
                    {


                        Double munits = (Int32.Parse(detail.Monday.Substring(3, 2)) + Int32.Parse(detail.Tuesday.Substring(3, 2)) + Int32.Parse(detail.Thursday.Substring(3, 2)) + Int32.Parse(detail.Wednesday.Substring(3, 2)) + Int32.Parse(detail.Friday.Substring(3, 2)));

                        Double hours = (Int32.Parse(detail.Monday.Substring(0, 2)) + Int32.Parse(detail.Tuesday.Substring(0, 2)) + Int32.Parse(detail.Thursday.Substring(0, 2)) + Int32.Parse(detail.Wednesday.Substring(0, 2)) + Int32.Parse(detail.Friday.Substring(0, 2)));


                        if (munits != 0)
                        {
                            Double charge = (munits / 60 + hours) / 8;
                            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                        }
                        else
                        {
                            Double charge = hours / 8;
                            tachhe.chargeConsomme = (charge + double.Parse(tachhe.chargeConsomme.Replace(".", ","))).ToString().Replace(",", ".");
                        }


                    }
                }
                _context.Entry(tachhe).State = EntityState.Modified;


            }

            try
            {

                await _context.SaveChangesAsync();
                return "ok";


            }
            catch (Exception e)
            {
                return "error save charge" + e;
            }

        }


        // DELETE: api/Imputations/5
        [HttpPost("Delete/Delete/{id}")]
        public async Task<ActionResult<Imputation>> DeleteImputation(string id)
        {
            var imputation = await _context.Imputations.FindAsync(id);
            if (imputation == null)
            {
                return NotFound();
            }

            _context.Imputations.Remove(imputation);
            await _context.SaveChangesAsync();

            return imputation;
        }
        [NonAction]
        private bool ImputationExists(string id)
        {
            return _context.Imputations.Any(e => e.Id == id);
        }
    }
}
