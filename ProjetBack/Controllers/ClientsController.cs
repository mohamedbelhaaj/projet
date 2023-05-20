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
    public class ClientsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public ClientsController(PilotageDBContext context)
        {
            _context = context;
        }

        //[HttpGet("GetByIdUser/{iduser}")]
        //public ActionResult GetByIdUser(string idUser)
        //{



        //    var projet = (
        //                 from client in _context.Clients
        //                 join projets in _context.Projets on client.Id equals projets.IdClient
        //                 join tache in _context.Taches on projets.Id equals tache.ProjetId
        //                 where tache.UserId == idUser
        //                 group new { client } by new { projets.IdClient, client.Nom, projets.Id, }
        //                 into grp
        //                 select new
        //                 {
        //                     Id = grp.Key.IdClient,
        //                     Nom = grp.Key.Nom,

        //                 }).ToList();

        //    if (projet == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(projet);
        //}
        //public async Task<I
        // GET: api/Clients
        [HttpPost("getStatByClient")]
        public IActionResult getStatByClient(List<string> clientIds)
        {

            var resultat = _context.Clients.Where(x => clientIds.Contains(x.Id)).Select(r => new
            {
                projectCount = r.ClientProjet.Count(),
                livraisonsCount= r.ClientProjet.Select(x=>x.Projet).SelectMany(x=>x.DetailLivraisons).Select(x=>x.ProjetLivraison).Distinct().Count(),
                ProjetCompatbleCount = r.ProjetEdps.Count(),

                r.DateCreation,
                
              
            }).FirstOrDefault();

            return Ok(resultat);
        }


        [HttpGet("GetClientsWithProjetComptable")]
        public IActionResult GetClientsWithProjetComptable()
        {

            var resultat = _context.Clients.Where(x => x.lockoutEnabled == true && x.ProjetEdps.Count>0 ).
                Select(r => new
            {
                Id = r.Id,
                Nom = r.Nom,

            }).ToList();


            //    var resultat = (from Client in _context.Clients
            //                  join project in _context.ProjetLivraisons on Client.Id equals project.ClientId into clientProject
            //                  select new
            //                  {
            //                      Client = Client,
            //                      projectCount = clientProject.Count(),
            //                  }).ToList();

            return Ok(new { resultat = resultat });

        }



        [HttpGet]
        public IActionResult GetClients()
        {

            var resultat = _context.Clients.Where(x => x.lockoutEnabled == true).Select(r => new
            {
                projectCount = r.ClientProjet.Count(),
                Id = r.Id,
                TypeClientId = r.TypeClientId,
                Adresse = r.Adresse,
                Nom = r.Nom,
                lockoutEnabled = r.lockoutEnabled,
   
            }).ToList();


            //    var resultat = (from Client in _context.Clients
            //                  join project in _context.ProjetLivraisons on Client.Id equals project.ClientId into clientProject
            //                  select new
            //                  {
            //                      Client = Client,
            //                      projectCount = clientProject.Count(),
            //                  }).ToList();

            return Ok(new { resultat = resultat });

        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            var client =  _context.Clients.Where(x => x.lockoutEnabled == true && x.Id==id).FirstOrDefault();

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutClient(string id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            // This is auto, no need out side
            client.DateCreation = DateTime.Now.ToString();
            client.DateDerniereModification = DateTime.Now.ToString();
            _context.Clients.Add(client);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
       [HttpPost("Delete/{id}")]
        public async Task<ActionResult<Client>> DeleteClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }
        //Ajouté le 8/11/2021 par wissem
        [HttpGet("ClientProjetExist")]
        public  IActionResult ClientProjetExist()
        {
            var client =  _context.Clients.Where(e => e.CodeProjets.Count() != 0).Select(r => new
            {
                projectCount = r.ClientProjet.Count(),
                Id = r.Id,
                TypeClientId = r.TypeClientId,
                Adresse = r.Adresse,
                Nom = r.Nom,
                lockoutEnabled = r.lockoutEnabled,

            }).ToList();

            return Ok(new { resultat = client });
        }



        [NonAction]
        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        //[HttpGet("projects")]
        //public IActionResult GetProjects(string id)
        //{
        //    List<ProjetLivraison> projects =  _context.ProjetLivraisonClients.Where(x => x.ClientId == id)
        //        .Select(x => x.ProjetLivraison).ToList();

        //    return Ok(projects);
        //}

    }

    public class ProjectWithDetails
    {
        
        public string IdProjet { get; set; }
        public string Nommenclature { get; set; }
        public string Description { get; set; }
        public string Publique { get; set; }
        public string DateCreation { get; set; }
        public string Createur { get; set; }
        public string DateDerniereModification { get; set; }
        public string Modificateur { get; set; }
        
        public List<DetailLivraison> Details { get; set; }
    }
    public class ClientWithProjects
    {
        public string Id { get; set; }
        public string Nom { get; set; }

        public string Adresse { get; set; }
        public List<ProjetLivraison> ProjsetLivraison { get; set; }
    }
}
