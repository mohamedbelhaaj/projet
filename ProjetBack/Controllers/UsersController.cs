using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Microsoft.AspNetCore.Cors;
using ProjetBack.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

using System.Data;

using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using ProjetBack.Dtos;
using Microsoft.Extensions.Options;
using Gateway.Dtos.Account;
using Tools.Tools;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PilotageDBContext _context;
        private readonly RSAEncrypt rSAEncrypt;
        private readonly AppSettings _appSettings;
        public UsersController(IConfiguration configure, PilotageDBContext context, IOptions<AppSettings> appSettings, RSAEncrypt rSAEncrypt)
        {
            _appSettings = appSettings?.Value;
            this._config = configure;
            _context = context;
            this.rSAEncrypt = rSAEncrypt;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel login)
        {
            try
            {



                string pass = await DecryptPassword(login.AdresseEmail);
                login.AdresseEmail = login.AdresseEmail.ToLower().Trim();
                var user = await _context.Users.Include(x => x.equipeUsers).Include(x => x.UserPermissions).ThenInclude(x => x.Permission)
                                               .FirstOrDefaultAsync(u => u.AdresseEmail.ToLower() == login.AdresseEmail && pass == login.MotDePasse);

                if (user == null)
                {
                    return Ok(new { codeRetour = 500 });
                }
                else if (!user.lockoutEnabled)
                {
                    return Ok(new { codeRetour = 501 });

                }

                else
                {
                    // ChangeTTMProjetLivraison();
                    user.lastConnexion = DateTime.Now;
                    user.confirmed = true;

                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                var tokenString = GenerateJSONWebToken(user);

                return Ok(new
                {

                    id = user.IdUser,
                    name = user.FullName,
                    email = user.AdresseEmail,
                    profile = user.ProfilId,
                    FirstImputation = user.FirstImputation,
                    LastImputation = user.LastImputation,
                    // EquipeId = user.,

                    role = RoleType.GetRole(user.Type),
                    permissions = user.UserPermissions.Select(x => x.Permission.Name).ToList(),
                    token = tokenString
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //public Boolean ChangeTTMProjetLivraison()
        //{
        //    var projets =  _context.ProjetLivraisons.Where(x => x.TTMId == null && x.DeliveryDate==null && x.StatusId== "Running").ToList();
        //    foreach (var item in projets)
        //    {
        //        if (item.InitialPlannedDate>DateTime.Now)
        //        {
        //            item.TTMId = "late";
        //        }

        //    }
        //    return true;
        //}

        
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: new Claim[] {
                  new Claim(ClaimTypes.Name,  userInfo.FullName ),
                  new Claim(ClaimTypes.NameIdentifier, userInfo.IdUser.ToString()),
                  new Claim(ClaimTypes.Email, userInfo.AdresseEmail),
                  new Claim("profile", userInfo.ProfilId??""),
                  new Claim(ClaimTypes.Role, RoleType.GetRole(userInfo.Type))
              },
              null,
              expires: DateTime.Now.AddMonths(6),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // GET: api/Users

        // [Authorize(Roles =RoleType.User)]
        [HttpGet("GetUsersFullName")]
        public IActionResult GetUsersFullName()
        {

            var resultat = _context.Users.Select(x => new
            {
                IdUser = x.IdUser,
         
                x.FullName,
     
            }).ToList();

            return Ok(resultat);

        }



        [HttpGet("GetUsersByDetailLivraison/{idDetailLivraison}")]
        public IActionResult GetUsersByDetailLivraison(string idDetailLivraison)
        {

            var resultat = (from u in _context.Users



                            join e in _context.EquipeUser on u.IdUser equals e.UserId

                            join p in _context.ProjetEquipes on e.EquipeId equals p.EquipeId
                            join d in _context.DetailLivraisons on p.ProjetId equals d.ProjetId
                           where     d.Id==idDetailLivraison 

                            select new {

                                   IdUser = u.IdUser,
                                   u.FullName,

                           }).ToList();

            return Ok(resultat);

        }






        [HttpGet("GetUsersWithEquipes")]
        public async Task<ActionResult> GetUsersWithEquipes()
        {

            //   var resultat = _context.Users.Include(x => x.equipeUsers).Include(x => x.equips).Where(x => x.lockoutEnabled == true).ToList();
            var resultat1 = await (from user in _context.Users
                                       //join EquipeUser in _context.EquipeUser on user.IdUser equals EquipeUser.EquipeId
                                       //join EquipeUser in _context. on user.IdUser equals EquipeUser.EquipeId


                                   select new
                                   {
                                       user.IdUser,
                                       user.FullName,
                                       equips = user.equips.Select(x => new { x.Id }).ToList(),
                                       equipeUsers = user.equipeUsers.Select(x => new {

                                           EquipeId = x.EquipeId,
                                           UserId = x.UserId
                                       }).ToList(),





                                   }).ToListAsync();




            return Ok(resultat1);

        }



        [HttpGet("GetUsersbyProjects")]
        public async Task<ActionResult> GetUsersbyProjects(string projectId)
        {

            //   var resultat = _context.Users.Include(x => x.equipeUsers).Include(x => x.equips).Where(x => x.lockoutEnabled == true).ToList();
            var resultat1 = await (from user in _context.Users
                                   //join EquipeUser in _context.EquipeUser on user.IdUser equals EquipeUser.EquipeId
                                   //join EquipeUser in _context. on user.IdUser equals EquipeUser.EquipeId
                                   

                                   select new
                                   {
                                       user.IdUser,
                                       user.FullName,
                                       equips =   user.equips.Select(x=>new { x.Id }).ToList(),
                                       equipeUsers = user.equipeUsers.Select(x => new {

                                           EquipeId=  x.EquipeId,
                                           UserId= x.UserId }).ToList(),





                                   }).ToListAsync();




            return Ok(resultat1);

        }



       


        [HttpGet("GetUsersByManagerWithEquipes/{idManager}")]

        public IActionResult GetUsersByManagerWithEquipes(string idManager)
        {

            var resultat = (from users in _context.Users
                            join equipeUser in _context.EquipeUser on users.IdUser equals equipeUser.UserId
                            join equips in _context.Equips on equipeUser.EquipeId equals equips.Id
                            where users.lockoutEnabled == true && (equips.ManagerId == idManager || users.IdUser==idManager)
                            select new
                            {
                                users.equips,
                                IdUser = users.IdUser,
                                FullName = users.FullName,
                                equipeUsers = users.equipeUsers.Where(x => x.Equipe.ManagerId == idManager || x.UserId == idManager).Select(x => new {

                                    EquipeId = x.EquipeId,
                                    UserId = x.UserId
                                }).Distinct()
                            }).Distinct().ToList();


            return Ok(resultat);

        }



        [HttpGet("GetUsersByManager/{idManager}")]

        public IActionResult GetUsersByManager(string idManager)
        {

            var resultat =  (from users in _context.Users  where   users.Type== UserType.User
                             join equipeUser in _context.EquipeUser on users.IdUser equals equipeUser.UserId
                             join equips in _context.Equips on equipeUser.EquipeId equals equips.Id

                             where (equips.ManagerId == idManager || users.IdUser == idManager)

                             select new
                            {
                                IdUser = users.IdUser,
                                Nom = users.Nom,
                                Prenom = users.Prenom,
                                users.FullName,
                                AdresseEmail = users.AdresseEmail,
                                ProfilId = users.ProfilId,
                                confirmed = users.confirmed,
                                type = users.Type,
                                lastConnexion = users.lastConnexion,
                                lockoutEnabled = users.lockoutEnabled,
                                managerofgroup = users.equips.Count,
                                expertise = users.expertise,


                            }).Distinct().ToList();


            return Ok(resultat);

        }

        [HttpGet("Managers")]
        
        public  IActionResult GetManager()
        {
            return Ok( _context.Users.Where(x=>x.Type!= UserType.User).ToList());
        }

        [HttpGet("GetCommercials")]

        public IActionResult GetCommercials()
        {
            return Ok(_context.Users.Where(x => x.Type == UserType.Commercial || x.IdUser== "4cc58011-0bba-4227-8226-6e3f82c9fe7f").Select(x=> new { x.FullName , x.IdUser}).ToList());
        }



        [HttpGet("getUserInfo/{id}")]

        public IActionResult getUserInfo(string id)
        {
            return Ok(_context.Users.Where(x => x.IdUser == id).Select(x=>new { 
            
            x.FullName,
            x.expertise,
            equipesName=x.equips.Select(y=>y.Nom).ToList(),
         x.ProfilId,
            x.DateCreation,
            x.DateDerniereModification,
            x.Telephone,
            equipeNames= x.equipeUsers.Select(y=>y.Equipe.Nom).ToList()

            
            }).FirstOrDefault());
        }

        [HttpGet("getLivraisonRunningForUser/{id}")]

        public IActionResult getLivraisonRunningForUser(string id)
        {
            var result = _context.Taches.Where(x => x.UserId == id).Select(x => x.detailLivraison).Select(x => x.ProjetLivraison).Distinct().Where(x => x.StatusId == "Running").Select(x =>new { projetName= x.ProjetName+" V"+x.Delivery ,x.PlannedDate}).OrderBy(x=>x.PlannedDate).ToList();


            return Ok( result );
        }

        [HttpGet("getUserStatProjectsInfo/{id}")]

        public IActionResult getUserStatProjectsInfo(string id)
        {


            var result = (from projet in _context.Projets
                          join equips in _context.ProjetEquipes on projet.Id equals equips.ProjetId
                          join equipsuser in _context.EquipeUser on equips.EquipeId equals equipsuser.EquipeId
                          where equipsuser.UserId == id || equipsuser.Equipe.ManagerId==id  
                          select new
                          {
                              projet.Id,
                              projet.Type,
                              projet.Nommenclature,
                              projet.Publique,
                              taches = projet.DetailLivraisons.SelectMany(s => s.taches).Where(f => f.UserId == id),
                              chargeTotale = projet.DetailLivraisons.SelectMany(s => s.taches).Sum(z => Convert.ToDouble(z.chargeConsomme)),
                          });

        //    var test = result.ToList();
            var projets = result.Where(x=>x.taches.Count()>0).Select(x => new
            {
                x.Id,
                x.Type,
                x.Nommenclature,
                x.Publique,
                chargeConsome = x.taches.Sum(z => Convert.ToDouble(z.chargeConsomme)),
                chargeEstime = x.taches.Sum(z => Convert.ToDouble(z.Charge)),
                chargeTotale = x.chargeTotale,
                premierImputation = x.taches.Where(y => y.premierImputation!=null).OrderBy(y => y.premierImputation).Select(x => x.premierImputation).First()

            });


            var projetss = projets.OrderBy(y => y.premierImputation).Distinct().ToList();
            return Ok(new
            {
                projets= projetss,
                nbProjetTotale = projetss.Count(),
                chrgeFormationExterne = projetss.Where(x => x.Type == "Formation Externe").ToList().Sum(x => x.chargeConsome),
                chargeTechnique = projetss.Where(x => x.Type == "Technique").ToList().Sum(x => x.chargeConsome),
                chargeTotale= projetss.Sum(x => x.chargeConsome),
                ChrgeFormationInterne = projetss.Where(x => x.Type == "Formation Interne").ToList().Sum(x => x.chargeConsome),
                ChrgeMangement = projetss.Where(x => x.Type == "Managment").ToList().Sum(x => x.chargeConsome),
                ChrgeExploitation = projetss.Where(x => x.Type == "Exploitation").ToList().Sum(x => x.chargeConsome),
                chargeAbcence = projetss.Where(x => x.Type == "Absence").ToList().Sum(x => x.chargeConsome),
                nbFormationExterne = projetss.Where(x => x.Type == "Formation Externe").Count(),
                nbTechnique = projetss.Where(x => x.Type == "Technique").Count(),
                nbFormationInterne = projetss.Where(x => x.Type == "Formation Interne").Count(),
                nbExploitation = projetss.Where(x => x.Type == "Exploitation").Count(),
                nbManagment = projetss.Where(x => x.Type == "Managment").Count(),
                nbAbcence = projetss.Where(x => x.Type == "Absence").Count()
            }
                ); 
        }
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            user.MotDePasse = rSAEncrypt.Decrypt(user.MotDePasse);

            return user;
        }
        [HttpGet("GetSimpleUsers")]
        public async Task<ActionResult<List<User>>> GetSimpleUser()
        {
            var user = await _context.Users.Where(x=>x.Type== UserType.User).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("GetUsersAndManagers")]
        public async Task<ActionResult<List<User>>> GetUsersAndManagers()
        {
            var user = await _context.Users.Where(x => x.Type == UserType.User || x.Type == UserType.Manager).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost("Update/{id}")]
        //public async Task<IActionResult> PutUser(string id, User user)
        //{
        //    if (id != user.IdUser)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //if(user.IdUser==null || user.IdUser=="")

            user.MotDePasse = rSAEncrypt.Encrypt(user.MotDePasse);
            var users = _context.Users.Where(x => x.AdresseEmail == user.AdresseEmail).FirstOrDefault();
            if(users!=null)
            {
                return Ok(new { codeRetour = 500 });
 

            }

            if (string.IsNullOrEmpty(user.IdUser))
            {
                user.DateCreation = DateTime.Now.ToString();
                _context.Users.Add(user);
            }
            else
            {
                _context.Entry(user).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutClient(string id, User user)
        {

            if (id != user.IdUser)
            {
                return BadRequest();
            }
            //if (user.Type != UserType.User && user.EquipeId != null)
            //    user.EquipeId = null;

            user.MotDePasse= EncryptPassword(user.MotDePasse);
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
          
                    throw;
                
            }


        }

        [HttpPost("disable/{id}")]
        public async Task<IActionResult> disableUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {

                user.lockoutEnabled = false;
                if (user.LastImputation == null)
                {
                    user.LastImputation = DateTime.Now;
                }
                _context.Entry(user).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
            }
            else return BadRequest();

        


        }

        //Login:

        //[HttpPost]
        //[Route("Login")]
        ////POST : /api/Users/Login
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.AdresseEmail);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.MotDePasse))
        //    {
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {

        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim("UserID",user.Id.ToString())
        //            }),
        //            Expires = DateTime.UtcNow.AddDays(1),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //        var token = tokenHandler.WriteToken(securityToken);
        //        return Ok(new { token });
        //    }
        //    else
        //        return BadRequest(new { message = "Email or password is incorrect." });
        //}


        // DELETE: api/Users/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {

            var resultat = _context.Users.Include(x => x.equipeUsers).Select(x => new
            {
                IdUser = x.IdUser,
                Nom = x.Nom,
                Prenom = x.Prenom,
                x.FirstImputation,
                x.FullName,
                AdresseEmail = x.AdresseEmail,
                ProfilId = x.ProfilId,
                confirmed = x.confirmed,
                type = x.Type,
                lastConnexion = x.lastConnexion,
                expertise = x.expertise,
                lockoutEnabled = x.lockoutEnabled,
                managerofgroup = x.equipeUsers.Where(y => y.role == "manager" && y.UserId == x.IdUser).Count()
            }).ToList();

            return Ok(resultat);

        }


        //[HttpGet("usersProject/{id}")]
        //public async Task<ActionResult<User>> GetUserByProject(string id)
        //{
        //    var result = await _context.Users.Include(x => x.Taches.Where(y => y.ProjetId == id)).ToListAsync();

        //    return Ok(result);
        //}

        [HttpGet("usersTasksByProject/{id}")]
        public async Task<ActionResult<User>> GetUserTasksByProject(string id,[FromQueryAttribute] string userId)
        {

            //var result = await _context.Projets.Include(c => c.Taches.FirstOrDefault().User).Where(c => c.Id == id)
            //        .Select(c => new
            //        {
            //            fullName = c.Taches.FirstOrDefault().User.FullName 
            //        })
            //        .ToListAsync();
            //return Ok(result);

            var dbResult = await _context.Taches.GroupJoin(_context.DetailImputations, t => t.Id, i => i.TacheId,
                                                          (task, details) => new { task, details })
                                                        .SelectMany(x => x.details.DefaultIfEmpty(),
                                                        (task, detail) => new { Task = task.task, Detail = detail })
                                                        .GroupJoin(_context.Users, td => td.Detail.Imputation.UserId, u => u.IdUser,
                                                        (taskDetail, users) => new { taskDetail, users })
                                                        .SelectMany(x => x.users.DefaultIfEmpty(),
                                                         (td, User) => new { td.taskDetail.Task, td.taskDetail.Detail, User })
                                                     //   .Where(x => x.Task.ProjetId == id)// && (string.IsNullOrWhiteSpace(userId) || (!string.IsNullOrWhiteSpace(userId) && x.User.IdUser == userId)))
                                                        .ToListAsync();

            var Result = dbResult.GroupBy(x => x.Task)
                                                     .Select(x => new
                                                     {
                                                         Task = new { x.Key.Id, x.Key.Description, x.Key.Charge, TotalTime = x.Sum(y => GetTimeInHours(y.Detail)) },
                                                         Details = x.GroupBy(y => new { y.User?.IdUser, y.User?.FullName }).Select(y => new { User = y.Key, TotalTime = y.Sum(z => GetTimeInHours(z.Detail)) }),

                                                     }).ToList();

            double GetTimeInHours(DetailImputation detail)
            {
                TimeSpan ConvertToTimeSpan(string time)
                {
                    if (string.IsNullOrWhiteSpace(time))
                        return new TimeSpan(0, 0, 0);
                    var timecomponents = time.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    int.TryParse(timecomponents[0], out int hours);
                    int.TryParse(timecomponents[1], out int minutes);

                    return new TimeSpan(hours, minutes, 0);
                }
                if (detail == null) detail = new DetailImputation();

                return ConvertToTimeSpan(detail.Monday).Add(ConvertToTimeSpan(detail.Tuesday))
                          .Add(ConvertToTimeSpan(detail.Wednesday)).Add(ConvertToTimeSpan(detail.Thursday))
                          .Add(ConvertToTimeSpan(detail.Friday))
                          .TotalHours;

            }
            return Ok(Result);
        }




        [HttpPost("GetUsersstatbyClient")]
        public async Task<ActionResult<User>> GetUsersClientComercant(Search searsh)
        {
            var resultat = (
                            from detail in _context.DetailImputations

                            from imputation in _context.Imputations.Where(x => x.Id == detail.ImputationId && x.UserId == searsh.idUser && (searsh.startDate==null || x.DateDebut >= searsh.startDate) && (searsh.endDate == null || x.DateFin <= searsh.endDate))
                            from tache in _context.Taches.Where(x => detail.TacheId == x.Id).DefaultIfEmpty()
                            from detailL in _context.DetailLivraisons.Where(x => tache.detailLivraisonId == x.Id).DefaultIfEmpty()
                            from projet in _context.Projets.Where(x => detailL.ProjetId == x.Id).DefaultIfEmpty()
                            from livraison in _context.ProjetLivraisons.Where(x => detailL.ProjetLivraisonId == x.Id).DefaultIfEmpty()
                            from client in _context.Clients.Where(x => livraison.ClientId == x.Id).DefaultIfEmpty()


                            select new
                            {
                                detail,
                                projetName = projet.Nommenclature,
                                projetId = projet.Id,
                                clientName = client.Nom,
                                clientId = client.Id,
                            }

                               ).ToList();


            // var group = resultat.GroupBy(x => x.clientId, x => x.detail, (clientId, ));

            var group = (from r in resultat
                         group r by (r.clientId, r.clientName) into g
                         select new
                         {
                             g.Key.clientName,
                             chargeParClient = g.Select(x=>x.detail).Sum(y => GetTimeInHours(y)),
               

                             projets =  
                                       (from i in g
                                        group i by  (i.projetId,i.projetName) into g2
                                        select new { 

                                            g2.Key.projetName,
                                            chargeParProjet= g2.Select(x => x.detail).Sum(y => GetTimeInHours(y)),

                                            }
                                        )
                          
                         }).ToList();












                //            resultat.GroupBy(x => x., x => x.detail,
                //(client, listDetails, projetId) => new
                //{
                //    clientName = client,
                //    projetId,
                //    chargeConsome = listDetails.Sum(x => x.Sum(y => GetTimeInHours(y.x))),



                //}).ToList();











            //});
            //var details = DetailWithLivraison.GroupBy(

            //    )




            //    Select(x => new
            //{
            //   client= x.detailLivraison.Select(y => y.ProjetLivraison).Select(y=>y.Client.Nom).ToList(),
            //  //details = x.detail,

            //    //clientComptable = x.detailLivraison.Select(y => y.ProjetLivraison).Select(y => y.projetEdp.client),
            //    //projet= x.detailLivraison.Select(y=>y.Projet)

            //}).ToList();


            //            var clientProject = details.GroupBy(
            //                x => x.client,
            //                x => x.details,
            //(client, listDetails) => new
            //{
            //    clientName = client,


            //    chargeConsome = listDetails.Sum(x => x.Sum(y => GetTimeInHours(y))),

            //});

            return Ok(group);

       
        }
        



        [NonAction]

        public  double GetTimeInHours(DetailImputation detail)
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


        [HttpPost("GetUsersStat")]
        public async Task<ActionResult<User>> GetUsersStat(Search searsh)
        {

            var resultdb = _context.Projets.Select(x => new
            {
                x.Id,
                x.Nommenclature,
                x.Type,
                x.Publique,

                listDetails = x.DetailLivraisons.SelectMany(y => y.taches).SelectMany(x => x.DetailImputations).Select(
                    f => new
                    {
                        f,
                        Charge = Convert.ToDouble(f.tache.Charge),

                        f.tache.premierImputation,
                        f.Imputation.DateDebut,
                        f.Imputation.DateFin,
                        f.Imputation.UserId,
                    }).Where(x => x.UserId == searsh.idUser && (searsh.startDate == null || x.DateDebut >= searsh.startDate) && (searsh.endDate == null || x.DateFin <= searsh.endDate)).Select(x => new {
                        x.f,
                        x.Charge,
                        x.premierImputation,
                    })

            });

            var result = resultdb.Where(x => x.listDetails.Count() > 0).ToList();

           


            var sommeTyppe = result.GroupBy(
                  x => x.Type,
                  x => x.listDetails,

                  (Type, imp) => new
                  {
                      Type,
                      chargeType = imp.Sum(x => x.Sum(z => GetTimeInHours(z.f))),
                      chargeTotale = result.Select(x => x.listDetails).Sum(x => x.Sum(z => GetTimeInHours(z.f))),
                      chargeEstimeTotale = result.Select(x => x.listDetails).Sum(x => x.Sum(z => z.Charge)),

                      nbType = result.Where(x => x.Type == Type).Count()

                  });

            var sommeProjet = result.GroupBy(
            x => x.Id,
            x => x.listDetails,
            (projetId, listDetails) => new
            {
                nommenclature = result.Where(x => x.Id == projetId).Select(x => x.Nommenclature),
                type = result.Where(x => x.Id == projetId).Select(x => x.Type),
                publique= result.Where(x => x.Id == projetId).Select(x => x.Publique),

                chargeConsome = listDetails.Sum(x => x.Sum(z => GetTimeInHours(z.f))),
                chargeEstime = listDetails.Sum(x => x.Sum(z => z.Charge)),

    //chargeestime = listDetails.Select(z => z.Select(y => y.tache).Sum(z => Convert.ToDouble(z.Charge))),
    //Select(z => z.Select(y => y.tache)).Sum(x => x.Sum(z => double.TryParse(z.Charge))),
    premierImputation = listDetails.OrderByDescending(x => x.Select(z => z.premierImputation)).FirstOrDefault().Select(x => x.premierImputation).FirstOrDefault()
            });



            double GetTimeInHours(DetailImputation detail)
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
            return Ok(new
            {
                sommeProjet = sommeProjet,
                sommeTyppe = sommeTyppe
            });
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            List<string> errors = new List<string>();
            string pass ;

            try
            {
                var user = _context.Users.Where(x => x.IdUser == model.id).FirstOrDefault();
                pass = await DecryptPassword(user.AdresseEmail);
                if (user == null)
                {
  
                    return Ok(new { error = "Utilisateur introuvable!" });
                }

                if (pass == model.OldPassword)
                {
                 
                    user.MotDePasse = EncryptPassword(model.NewPassword);
                    var res = _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(new { error = false, res=res });

                }
    
                 return Ok(new { error = "Le mot de passe est incorrect" });

            }
            catch (Exception ex)
            {
                return Ok(new { error = "Problème de connexion" });

      
           
            }


        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(string userName)
        {
            try
            {
       
                if (userName == null)
                    return BadRequest("UserName ou Email est requis");
                User user = null;

          
            user = _context.Users.Where(x => x.AdresseEmail.ToLower() == userName).FirstOrDefault(); ;
             
                if (user == null)
                    return BadRequest("Utilisateur introuvable!");




                // compute the new hash string
                var newPassword = GenerateRandomPassword();
             string   Password = rSAEncrypt.Encrypt(newPassword);

                user.MotDePasse = Password;
                var currentTime = DateTime.Now;

                var message = "<div style=''>Bonjour <strong>" + user.Nom + " " + user.Prenom + "</strong>," + " <p> Bienvenue sur la plateforme System info , votre mot de passe a été changé.</p> <p> Votre nouveau mot de passe  est : <strong>" + newPassword + "</strong></p>" + " <p><a href='http://192.168.1.40/PortailSysInfo'> Cliquez ici pour vous connecter </a>.</p></div>";
                var subject = "Récupération mot de passe";
                SendEmail(user.AdresseEmail, message, subject);


                user.ResetPassword = currentTime;
                user.MotDePasse = Password;
                var res = _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                // HttpContext.Session.SetInt32(user.UserName,1);

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest("Problème de connexion "+ ex);
            }

        }
        [NonAction]
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        [NonAction]
        public string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 12,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = false,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$?_-"                        // non-alphanumeric
          };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
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


        [HttpPost("SendMail")]
        public void SendEmails(string email, string messageBody, string subject)
        {   
            SmtpClient client = new SmtpClient(_appSettings.Host, _appSettings.EmailPort);

            client.EnableSsl = _appSettings.EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_appSettings.EmailSender, _appSettings.Password);
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add("wkhaskhoussy@gmail.com");
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









        [HttpPost("Encrypts")]
        public async Task<IActionResult> EncryptPasswords()
        {
  
         
            string password;

            var result = await _context.Users.ToListAsync();
            foreach (var a in result)
            {
               
              password=  rSAEncrypt.Encrypt(a.MotDePasse);
                var user = await _context.Users.FindAsync(a.IdUser);
                user.MotDePasse = password;
                _context.Entry(user).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }

            }
           


         

            return Ok();

        }

        [HttpPost("Encrypt")]
        public  string EncryptPassword(string password)
        {


            

                password = rSAEncrypt.Encrypt(password);



            return password;

        }



        [HttpPost("Decrypt")]
        public async Task<string> DecryptPassword(string mail)
        {
          
            var user = await _context.Users.Where(e=>e.AdresseEmail.Equals(mail)).FirstOrDefaultAsync();
            if(user==null)
            {
                return "not found";
            }
            try
            {
                string pass = rSAEncrypt.Decrypt(user.MotDePasse);
            
                return pass;

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
           

        }




        [HttpGet("ManagerByUser/{id}")]
        public async Task<List<string>> GetManagerByUser(string id)
        {

            // var dbresult = await _context.EquipeUser.
            //     Include(e => e.User).
            //     Where(e => e.UserId.
            //     Equals(id)).
            //     Select(e => e.EquipeId).ToListAsync();

            // var result =  await  _context.Users.Include(e => e.equips).FirstOrDefaultAsync(e=>e.IdUser==e.equips.FirstOrDefault(e => dbresult.Contains(e.Id)).ManagerId);


            //// var result = await _context.Equips.Include(e => e.).FirstOrDefaultAsync(e => dbresult.Contains(e.Id));
            // //var Result = dbResult.GroupBy(x => x.Task)
            // //                                         .Select(x => new
            // //                                         {
            // //                                             Task = new { x.Key.Id, x.Key.Description, x.Key.Charge, TotalTime = x.Sum(y => GetTimeInHours(y.Detail)) },
            // //                                             Details = x.GroupBy(y => new { y.User?.IdUser, y.User?.FullName }).Select(y => new { User = y.Key, TotalTime = y.Sum(z => GetTimeInHours(z.Detail)) }),

            // //                                         }).ToList();
            // var dbresult = await _context.EquipeUser.
            //Include(e => e.User).ThenInclude(e => e.equips).
            //Where(e => e.UserId.
            //Equals(id)).FirstOrDefaultAsync(e=>e.User.IdUser==e.User.equips.FirstOrDefault(e=>e.ManagerId==e.equipeUsers.);
            //  var result2 = dbresult.FirstOrDefault(e => e.User.IdUser == e.EquipeId).Equipe.ManagerId;

            //var result = await _context.Users.Include(e => e.equips).FirstOrDefaultAsync(e => e.IdUser == e.equips.FirstOrDefault(e => dbresult.Contains(e.Id)).ManagerId);


            //return ;


            var dbresult = await _context.EquipeUser.
           Include(e => e.User).Where(e
           => e.UserId == id).Select(e => e.EquipeId).ToListAsync();
            var result2 = await _context.Equips.Include(e => e.Manager).Where(e => dbresult.Contains(e.Id)).Select(e => e.Manager.AdresseEmail).Distinct().ToListAsync();
             //   mails.Add(result.AdresseEmail);





            return result2;
        }






    }

}

