using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using ProjetBack.Models;
using ProjetBack.Dtos;

namespace ProjetBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetEdpsController : ControllerBase
    {
        private readonly PilotageDBContext _context;

        public ProjetEdpsController(PilotageDBContext context)
        {
            _context = context;
        }

        double GetTimeInHours(DetailImputation detail)
        {
            TimeSpan ConvertToTimeSpan(string time)
            {
                if (string.IsNullOrWhiteSpace(time))
                    return new TimeSpan(0, 0, 0);
                var timecomponents = time.Split(':', StringSplitOptions.RemoveEmptyEntries);
            int    hours= Convert.ToInt32(timecomponents[0]);
           int     minutes = Convert.ToInt32(timecomponents[1]);

                return new TimeSpan(hours, minutes, 0) / 8;
            }
            if (detail == null) detail = new DetailImputation();

            return ConvertToTimeSpan(detail.Monday).Add(ConvertToTimeSpan(detail.Tuesday))
                      .Add(ConvertToTimeSpan(detail.Wednesday)).Add(ConvertToTimeSpan(detail.Thursday))
                      .Add(ConvertToTimeSpan(detail.Friday))
                      .TotalHours;

        }


        [HttpPost("StatChargeParUserProjetCompatbleExcel")]

        public async Task<IActionResult> StatChargeParUserProjetCompatbleExcel(StatProjetComptableUsersDto model)
        {



            try
            {
                int offset = (model.page * model.size);

                var dbresult = (from di in _context.DetailImputations



                                join i in _context.Imputations on di.ImputationId equals i.Id
                                join u in _context.Users on i.UserId equals u.IdUser
                                join t in _context.Taches on di.TacheId equals t.Id
                                join dl in _context.DetailLivraisons on t.detailLivraisonId equals dl.Id
                                join pl in _context.ProjetLivraisons on dl.ProjetLivraisonId equals pl.Id
                                join pc in _context.ProjetEdps on pl.projetEdpId equals pc.id into joinedpc
                                from pc in joinedpc.DefaultIfEmpty()
                                join c in _context.Clients on pc.clientId equals c.Id into joinedc
                                from c in joinedc.DefaultIfEmpty()
                                join cp in _context.CodeProjet on pc.CodeProjetsId equals cp.id into joinedcp
                                from cp in joinedcp.DefaultIfEmpty()
                                where


                               (model.DateDebut == "" || (i.TuesdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date) || i.WednesdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.FridayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.ThursdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.AddDays(5).Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.AddDays(6).Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.Date >= Convert.ToDateTime(model.DateDebut).Date)
                             && (model.DateFin == "" || i.ThursdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.WednesdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.TuesdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.mondayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.DateFin <= Convert.ToDateTime(model.DateFin).Date)

                                   &&
                                    (model.ListUsers.Count == 0 || model.ListUsers.Contains(u.IdUser))
                                     && (model.ListClient.Count == 0 || model.ListClient.Contains(c.Id))



                                select new
                                {



                                    di,
                                    i.mondayDate,
                                    i.TuesdayDate,
                                    i.ThursdayDate,
                                    i.FridayDate,
                                    i.WednesdayDate,
                                    pl.ProjetName,
                                    pl.Id,
                                    cp.id,
                                    di.Monday,
                                    di.Tuesday,
                                    di.Friday,
                                    di.Wednesday,
                                    di.Thursday,
                                    u.IdUser,
                                    nom = u.Prenom + " " + u.Nom,
                                    projetLivraisonId = pl.Id,
                                    projetLivraison = pl.ProjetName + " V" + pl.Delivery,
                                    projetComptable = cp.Intitule + " | " + pc.Nom,
                                    codeAnalityque = cp.Numero + " | " + pc.CodeProjet,
                                    client = c.Nom,

                                });

                var resultMonday = await (from monday in dbresult

                                          group monday by new { monday.mondayDate, monday.IdUser, monday.projetLivraisonId, monday.nom, monday.projetLivraison, monday.client, monday.codeAnalityque, monday.projetComptable } into g
                                          select new
                                          {
                                               charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Monday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Monday.Substring(3, 2))) /60),
                                              g.Key.nom,
                                              g.Key.projetLivraison,
                                              date = g.Key.mondayDate.Value.Date,
                                              g.Key.codeAnalityque,
                                              g.Key.projetComptable,
                                              year = g.Key.mondayDate.Value.Year,
                                              g.Key.client,
                                              monnth = g.Key.mondayDate.Value.Month,
                                          }).ToListAsync();


                var resultuesday = await (from day in dbresult

                                          group day by new { day.TuesdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                          select new
                                          {
                                               charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Tuesday.Substring(3, 2))) /60),
                                              g.Key.nom,
                                              g.Key.projetLivraison,
                                              date = g.Key.TuesdayDate.Value.Date,
                                              g.Key.codeAnalityque,
                                              g.Key.projetComptable,
                                              year = g.Key.TuesdayDate.Value.Year,
                                              g.Key.client,
                                              monnth = g.Key.TuesdayDate.Value.Month,
                                          }).ToListAsync();


                var resulWednesday = await (from day in dbresult

                                            group day by new { day.WednesdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                            select new
                                            {
                                                 charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Wednesday.Substring(3, 2))) /60),
                                                g.Key.nom,
                                                g.Key.projetLivraison,
                                                date = g.Key.WednesdayDate.Value.Date,
                                                g.Key.codeAnalityque,
                                                g.Key.projetComptable,
                                                year = g.Key.WednesdayDate.Value.Year,
                                                g.Key.client,
                                                monnth = g.Key.WednesdayDate.Value.Month,
                                            }).ToListAsync();

                var resulFriday = await (from day in dbresult

                                         group day by new { day.projetLivraisonId, day.FridayDate, day.IdUser,  day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                         select new
                                         {
                                              charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Friday.Substring(3, 2))) /60),
                                             g.Key.nom,
                                             g.Key.projetLivraison,
                                             date = g.Key.FridayDate.Value.Date,
                                             g.Key.codeAnalityque,
                                             g.Key.projetComptable,
                                             year = g.Key.FridayDate.Value.Year,
                                             g.Key.client,
                                             monnth = g.Key.FridayDate.Value.Month,
                                         }).ToListAsync();
                var resulThuresday = await (from day in dbresult

                                            group day by new { day.ThursdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                            select new
                                            {
                                                 charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Thursday.Substring(3, 2))) /60),
                                                g.Key.nom,
                                                g.Key.projetLivraison,
                                                date = g.Key.ThursdayDate.Value.Date,
                                                g.Key.codeAnalityque,
                                                g.Key.projetComptable,
                                                year = g.Key.ThursdayDate.Value.Year,
                                                g.Key.client,
                                                monnth = g.Key.ThursdayDate.Value.Month,
                                            }).ToListAsync();
                resulFriday.AddRange(resulThuresday);
                resulFriday.AddRange(resulWednesday);
                resulFriday.AddRange(resultuesday);
                resulFriday.AddRange(resultMonday);
                var finalResult = resulFriday.Where(x => (model.DateDebut == "" || x.date >= Convert.ToDateTime(model.DateDebut).Date)
                && (model.DateFin == "" || x.date <= Convert.ToDateTime(model.DateFin).Date)
                && x.charge != 0).OrderByDescending(x => x.date);
            
                return Ok(new {  data = finalResult });
            }
            catch (Exception ex)
            {

                return BadRequest(new JsonResult(ex.Message));
            }

        }




        [HttpPost("StatChargeParUserProjetCompatble")]

        public async Task<IActionResult> StatChargeParUserProjetCompatble(StatProjetComptableUsersDto model)
        {



            try
            {

                var value = "02:30";
                var heur = Convert.ToDouble(value.Substring(0, 2)) ;

                var menute = value.Substring(3, 2);
                var menut4e = Convert.ToDouble(menute) ;
                var menute5 = (menut4e /60);
               var  somme = heur + menute5;
                List<double> list = new List<double>() { 0.5,0, 0.5,0,0, 0.5 }; ;
           


                var listSomme = list.Sum(x => x);


                int offset = (model.page * model.size);
                
                var dbresult = ( from di in _context.DetailImputations


                                 
                                      join i in _context.Imputations on di.ImputationId equals i.Id
                                      join u in _context.Users on i.UserId equals u.IdUser
                                      join t in _context.Taches on di.TacheId equals t.Id
                                      join dl in _context.DetailLivraisons on t.detailLivraisonId equals dl.Id
                                      join pl in _context.ProjetLivraisons on dl.ProjetLivraisonId equals pl.Id
                                      join pc in _context.ProjetEdps on pl.projetEdpId equals pc.id into joinedpc
                                      from pc in joinedpc.DefaultIfEmpty()
                                      join c in _context.Clients on pc.clientId equals c.Id into joinedc
                                      from c in joinedc.DefaultIfEmpty()
                                      join cp in _context.CodeProjet on pc.CodeProjetsId equals cp.id into joinedcp
                                      from cp in joinedcp.DefaultIfEmpty()
                                      where
                       

                                     (model.DateDebut == "" || (i.TuesdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date) || i.WednesdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.FridayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.ThursdayDate.Value.Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.AddDays(5).Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.AddDays(6).Date == Convert.ToDateTime(model.DateDebut).Date || i.DateDebut.Date >= Convert.ToDateTime(model.DateDebut).Date)
                                   &&  (model.DateFin == ""  || i.ThursdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.WednesdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.TuesdayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.mondayDate.Value.Date == Convert.ToDateTime(model.DateFin).Date || i.DateFin <= Convert.ToDateTime(model.DateFin).Date)

                                         &&
                                          (model.ListUsers.Count == 0 || model.ListUsers.Contains(u.IdUser))
                                           && (model.ListClient.Count == 0 ||   model.ListClient.Contains(c.Id))
                           

                         
                                 select new
                                      {

                            
                                     
                                     di, 
                                     i.mondayDate, 
                                     i.TuesdayDate,
                                     i.ThursdayDate,
                                     i.FridayDate, 
                                     i.WednesdayDate, 
                                     pl.ProjetName, 
                                     pl.Id,
                                    cp.id,
                                    di.Monday,
                                    di.Tuesday,
                                    di.Friday, 
                                    di.Wednesday, 
                                    di.Thursday,
                                    u.IdUser,
                                     nom = u.Prenom + " " + u.Nom,
                                     projetLivraisonId= pl.Id,
                                     projetLivraison = pl.ProjetName + " V" + pl.Delivery,
                                     projetComptable = cp.Intitule + " | " + pc.Nom,
                                     codeAnalityque = cp.Numero + " | " +pc.CodeProjet,
                                     client = c.Nom,
                                         
                                                                  });

                var resultMonday = await( from monday in dbresult

                                   group monday by new { monday.mondayDate, monday.IdUser, monday.projetLivraisonId, monday.nom, monday.projetLivraison, monday.client, monday.codeAnalityque , monday.projetComptable } into g
                                   select new
                                   {
                                        charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Monday.Substring(0, 2))) + (double) g.Sum(x => Convert.ToDouble(x.di.Monday.Substring(3, 2))) /60),
                                       g.Key.nom,
                                       g.Key.projetLivraison,
                                       date  =g.Key.mondayDate.Value.Date,
                                       g.Key.codeAnalityque,
                                       g.Key.projetComptable,
                                       year = g.Key.mondayDate.Value.Year,
                                      g.Key.client,
                                       monnth = g.Key.mondayDate.Value.Month,
                                   }).ToListAsync();


                var resultuesday = await (from day in dbresult

                                   group day by new { day.TuesdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                   select new
                                   {
                                       charge =(double) ( g.Sum(x => Convert.ToDouble(x.di.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Tuesday.Substring(3, 2))) /60),
                                       g.Key.nom,
                                       g.Key.projetLivraison,
                                       date = g.Key.TuesdayDate.Value.Date,
                                       g.Key.codeAnalityque,
                                       g.Key.projetComptable,
                                       year = g.Key.TuesdayDate.Value.Year,
                                       g.Key.client,
                                       monnth = g.Key.TuesdayDate.Value.Month,
                                   }).ToListAsync();


                var resulWednesday = await(from day in dbresult

                                   group day by new { day.WednesdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                   select new
                                   {
                                       charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Wednesday.Substring(3, 2))) /60),
                                       g.Key.nom,
                                       g.Key.projetLivraison,
                                       date = g.Key.WednesdayDate.Value.Date,
                                       g.Key.codeAnalityque,
                                       g.Key.projetComptable,
                                       year = g.Key.WednesdayDate.Value.Year,
                                       g.Key.client,
                                       monnth = g.Key.WednesdayDate.Value.Month,
                                   }).ToListAsync();

                var resulFriday =await( from day in dbresult

                                     group day by new { day.FridayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                     select new
                                     {
                                          charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Friday.Substring(3, 2))) /60),
                                         g.Key.nom,
                                         g.Key.projetLivraison,
                                         date = g.Key.FridayDate.Value.Date,
                                         g.Key.codeAnalityque,
                                         g.Key.projetComptable,
                                         year = g.Key.FridayDate.Value.Year,
                                         g.Key.client,
                                         monnth = g.Key.FridayDate.Value.Month,
                             }).ToListAsync();
                var resulThuresday =await (from day in dbresult

                                  group day by new { day.ThursdayDate, day.IdUser, day.projetLivraisonId, day.nom, day.projetLivraison, day.client, day.codeAnalityque, day.projetComptable } into g
                                  select new
                                  {
                                       charge = (double)(g.Sum(x => Convert.ToDouble(x.di.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.di.Thursday.Substring(3, 2))) /60),
                                      g.Key.nom,
                                      g.Key.projetLivraison,
                                      date = g.Key.ThursdayDate.Value.Date,
                                      g.Key.codeAnalityque,
                                      g.Key.projetComptable,
                                      year = g.Key.ThursdayDate.Value.Year,
                                      g.Key.client,
                                      monnth = g.Key.ThursdayDate.Value.Month,
                                  }).ToListAsync();
                resulFriday.AddRange(resulThuresday);
                resulFriday.AddRange(resulWednesday);  
                resulFriday.AddRange(resultuesday);
                resulFriday.AddRange(resultMonday);
                var finalResult = resulFriday.Where(x =>( model.DateDebut == "" || x.date >= Convert.ToDateTime(model.DateDebut).Date)
                &&(model.DateFin == "" || x.date <= Convert.ToDateTime(model.DateFin).Date)
                && x.charge != 0);
                var nbTotalResults = finalResult.Count();
                var data = finalResult.OrderByDescending(x => x.date).Skip(offset).Take(model.size).ToList();

                return Ok(new { nbTotalResults, data }); ;

            }
            catch (Exception ex)
            {
              
                return BadRequest(new JsonResult(ex.Message));
            }

        }





        [HttpGet("StatChargeByUser/{projetId}")]
        public IActionResult StatChargeByUser(string projetId)
        {
            try
            {

                var resultat = (from user in _context.Users

                                join tache in _context.Taches on user.IdUser equals tache.UserId

                                join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                                join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
                                join edp in _context.ProjetEdps on Livraison.projetEdpId equals edp.id
                                join di in _context.DetailImputations on tache.Id equals di.TacheId

                                where edp.id == projetId
                                //group new {di.Monday, di.Tuesday, di.Thursday, di.Wednesday, di.Friday} by new
                                //{
                                //    user.FullName,
                                //    user.expertise,
                                //} into g 
                                select new
                                {
                                    //g.Key.FullName,
                                    //g.Key.expertise,
                                    user.FullName,
                                    user.expertise,
                                    //   charge = edp.ProjetLivraisons.SelectMany(x => x.DetailLivraisons).SelectMany(x => x.taches).Where(x => x.UserId == user.IdUser).Sum(x => Convert.ToDouble(x.chargeConsomme))

                                    Monday = (Convert.ToDouble(di.Monday.Substring(0, 2)) + Convert.ToDouble(di.Monday.Substring(3, 2)) / 60) / 8,
                                    Tuesday = (Convert.ToDouble(di.Tuesday.Substring(0, 2)) + Convert.ToDouble(di.Tuesday.Substring(3, 2)) / 60) / 8,
                                    Friday = (Convert.ToDouble(di.Friday.Substring(0, 2)) + Convert.ToDouble(di.Friday.Substring(3, 2)) / 60) / 8,
                                    Wednesday = (Convert.ToDouble(di.Wednesday.Substring(0, 2)) + Convert.ToDouble(di.Wednesday.Substring(3, 2)) / 60) / 8,
                                    Thursday = (Convert.ToDouble(di.Thursday.Substring(0, 2)) + Convert.ToDouble(di.Thursday.Substring(3, 2)) / 60) / 8,

                                    //charge = (g.Sum(x=> Convert.ToDouble(x.Monday.Substring(0, 2)))+ g.Sum(x => Convert.ToDouble(x.Monday.Substring(3, 2))) / 60) / 8+
                                    //(g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(3, 2))) / 60) / 8 +
                                    //(g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(3, 2))) / 60) / 8 +
                                    //(g.Sum(x => Convert.ToDouble(x.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Thursday.Substring(3, 2))) / 60) / 8 +
                                    //(g.Sum(x => Convert.ToDouble(x.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Friday.Substring(3, 2))) / 60) / 8 

                                }).ToList();





                var groupedByuser = (from r in resultat
                                     group new { r.Monday, r.Tuesday, r.Thursday, r.Wednesday, r.Friday } by new
                                     {
                                         r.FullName,
                                         r.expertise,
                                     } into g
                                     select new
                                     {
                                         g.Key.expertise,
                                         g.Key.FullName,
                                         charge = g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday),
                                     }).ToList().OrderBy(x => x.expertise);
                return Ok(groupedByuser);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }




//        [HttpGet("StatChargeByUser/{projetId}")]
//        public IActionResult StatChargeByUser(string projetId)
//        {
//            try { 

//            var resultat = (from user in _context.Users



            
//                            join tache in _context.Taches on user.IdUser equals tache.UserId

//                            join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
//                            join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
//                            join edp in _context.ProjetEdps on Livraison.projetEdpId equals edp.id

//                            where edp.id == projetId 
//                            select new
//                            {
//                                user.FullName,
//                                user.expertise,
//                                charge = edp.ProjetLivraisons.SelectMany(x=>x.DetailLivraisons).SelectMany(x => x.taches).Where(x=>x.UserId== user.IdUser).Sum(x => Convert.ToDouble(x.chargeConsomme))


//                            }).Distinct().ToList().OrderBy(x => x.expertise);


//            return Ok(resultat);
//        }
//            catch (Exception e)
//            {

//                return BadRequest(e);
//    }
//}

        [HttpGet("StatChargeByProfile/{projetId}")]
        public IActionResult StatChargeByProfile(string projetId)
        {




            var dbresult =  (from di in _context.DetailImputations



                          //  join i in _context.Imputations on di.ImputationId equals i.Id
                            join t in _context.Taches on di.TacheId equals t.Id
                            join p in _context.profileUser on t.UserId equals p.userId

                            join dl in _context.DetailLivraisons on t.detailLivraisonId equals dl.Id
                            join pl in _context.ProjetLivraisons on dl.ProjetLivraisonId equals pl.Id
                            join edp in _context.ProjetEdps on pl.projetEdpId equals edp.id into joinedpc
                            from edp in joinedpc.DefaultIfEmpty()
        
        
                            where edp.id== projetId 

                            select new
                            {

                                budgetConfirme= Convert.ToDouble(edp.budgetConfirme) + Convert.ToDouble(edp.budgetConfirmeRallonge)  ,
                                budgetDirection= Convert.ToDouble(edp.budgetDirection)+ Convert.ToDouble(edp.budgetDirectionRallonge),
                                budgetGP= Convert.ToDouble(edp.budgetGP)+ Convert.ToDouble(edp.budgetGPRallonge),
                                budgetJunior=Math.Abs(Convert.ToDouble(edp.budgetJunior) + Convert.ToDouble(edp.budgetJuniorRallonge)) ,
                                budgetSenior = Convert.ToDouble(edp.budgetSenior)+ Convert.ToDouble(edp.budgetSeniorRallonge),
                                budgetValidation= Convert.ToDouble(edp.budgetValidation)+ Convert.ToDouble(edp.budgetValidationRallonge),
                                p.profileId,
                                t.UserId,
                                Monday = (Convert.ToDouble(di.Monday.Substring(0, 2)) + Convert.ToDouble(di.Monday.Substring(3, 2)) / 60) / 8,
                                Tuesday = (Convert.ToDouble(di.Tuesday.Substring(0, 2)) + Convert.ToDouble(di.Tuesday.Substring(3, 2)) / 60) / 8,
                                Friday = (Convert.ToDouble(di.Friday.Substring(0, 2)) + Convert.ToDouble(di.Friday.Substring(3, 2)) / 60) / 8,
                                Wednesday = (Convert.ToDouble(di.Wednesday.Substring(0, 2)) + Convert.ToDouble(di.Wednesday.Substring(3, 2)) / 60) / 8,
                                Thursday = (Convert.ToDouble(di.Thursday.Substring(0, 2)) + Convert.ToDouble(di.Thursday.Substring(3, 2)) / 60) / 8,

                            }).AsEnumerable();






            var result2 = from pr in dbresult
                          group pr by new { pr.profileId, pr.UserId , pr.budgetConfirme  , pr.budgetGP, pr.budgetDirection, pr.budgetJunior, pr.budgetSenior, pr.budgetValidation }  into g
                          select new
                          {
                             g.Key.UserId,
                             g.Key.profileId,

                              g.Key.budgetConfirme,
                              g.Key.budgetGP,
                              g.Key.budgetDirection,
                              g.Key.budgetJunior,
                              g.Key.budgetSenior,
                              g.Key.budgetValidation,
                              charge = g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday),
                              cout = (g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday))
                             * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault())
                          };


            var result3 = from pr in result2
                          group pr by new { pr.profileId, pr.budgetConfirme, pr.budgetGP, pr.budgetDirection, pr.budgetJunior, pr.budgetSenior, pr.budgetValidation } into g
                          select new
                          {
                              g.Key.profileId,
                              chargeEstime = (g.Key.profileId == "confirmé"  ? g.Key.budgetConfirme
                             : (g.Key.profileId == "Direction" ? g.Key.budgetDirection
                             : (g.Key.profileId == "GP" ? g.Key.budgetGP
                             : (g.Key.profileId == "junior" ? g.Key.budgetJunior
                             : (g.Key.profileId == "sénior" ? g.Key.budgetSenior
                             : (g.Key.profileId == "Validation" ? g.Key.budgetValidation
                            : 0)))))
                             ),
                              coutEstime = (g.Key.profileId == "confirmé" ? g.Key.budgetConfirme 
                             : (g.Key.profileId == "Direction" ? g.Key.budgetDirection
                             : (g.Key.profileId == "GP" ? g.Key.budgetGP
                             : (g.Key.profileId == "junior" ? g.Key.budgetDirection
                             : (g.Key.profileId == "sénior" ? g.Key.budgetJunior
                             : (g.Key.profileId == "Validation" ? g.Key.budgetValidation
                            : 0)))))
                             )* Convert.ToDouble(_context.profile.Where(x =>x.profileId== g.Key.profileId).Select(x=>x.budget).FirstOrDefault()),
                              nbusers = g.Count(),
                              charge= g.Sum(x => Convert.ToDouble(x.charge)),
                              cout = g.Sum(x => Convert.ToDouble(x.cout)
                              )





                          };


            //var resultat =  (
            //                from tache in _context.Taches

            //                join user in _context.Users on tache.UserId equals user.IdUser
            //                join profileUser in _context.profileUser on user.IdUser equals profileUser.userId

            //                join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
            //                join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
            //                join edp in _context.ProjetEdps on Livraison.projetEdpId equals edp.id

            //                where edp.id == projetId
            //              //  group new { tache  }  by { profileUser.profileId, edp.id } into g

            //                select new
            //                {
            //                    budgetConfirme=     edp.budgetConfirme+ edp.budgetConfirmeRallonge,
            //                    budgetDirection=    edp.budgetDirection+ edp.budgetDirectionRallonge,
            //                    budgetGP=   edp.budgetGP+ edp.budgetGPRallonge,
            //                    budgetJunior=      edp.budgetJunior+ edp.budgetJuniorRallonge,
            //                    budgetSenior=       edp.budgetSenior+ edp.budgetSeniorRallonge,
            //                    budgetValidation=     edp.budgetValidation+ edp.budgetValidationRallonge,

            //                    tache.chargeConsomme,
            //                 //   user.IdUser.ToList(),




            //                    // charge = g.Sum(x => Convert.ToDouble(x.tache.chargeConsomme)),
            //                    // cout = g.Sum(x => Convert.ToDouble(x.tache.chargeConsomme)) * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key).Select(x => x.budget).FirstOrDefault()),
            //                    ////user.Count()
            //                } ).ToList();

            return Ok(result3);

            //var Users = resultat.Select(x => x.User);
            //var taches = resultat.Select(x => x.tache);
            //return Ok(new
            //{
            //    tachesbyGP = taches.Where(x => x.User.expertise == "GP").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    tachesbyValidation = taches.Where(x => x.User.expertise == "Validation").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    tachesbyDirection = taches.Where(x => x.User.expertise == "Direction").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    tachesbyConfirme = taches.Where(x => x.User.expertise == "Confirmé").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    tachesbyJunior = taches.Where(x => x.User.expertise == "junior").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    tachesbySernior = taches.Where(x => x.User.expertise == "sénior").Sum(x => Convert.ToDouble(x.chargeConsomme)),
            //    countGP = Users.Where(x => x.expertise == "GP").Distinct().Count(),
            //    countConfirme = Users.Where(x => x.expertise == "Confirmé").Distinct().Count(),
            //    countJunior = Users.Where(x => x.expertise == "junior").Distinct().Count(),
            //    countSernior = Users.Where(x => x.expertise == "sénior").Distinct().Count(),
            //    countDirection = Users.Where(x => x.expertise == "Direction").Distinct().Count(),
            //  countValidation = Users.Where(x => x.expertise == "Validation").Distinct().Count(),

                //usersGP = Users.Where(x => x.expertise == "GP").Distinct().Select(x => x.FullName).ToList(),
                //usersConfirme = Users.Where(x => x.expertise == "Confirmé").Distinct().Select(x => x.FullName).ToList(),
                //usersJunior = Users.Where(x => x.expertise == "junior").Distinct().Select(x => x.FullName).ToList(),
                //usersSernior = Users.Where(x => x.expertise == "sénior").Distinct().Select(x => x.FullName).ToList(),
                //usersDirection = Users.Where(x => x.expertise == "Direction").Distinct().Select(x=>x.FullName).ToList(),
                //usersValidation = Users.Where(x => x.expertise == "Validation").Distinct().Select(x => x.FullName).ToList()
             

     
        }
        [HttpPost("satatByClient")]
        public IActionResult satatByClient(List<string> ClientId)
        {
            try
            {
                var resultat = (from edp in _context.ProjetEdps
                                where ( ClientId.Count == 0||ClientId.Contains(edp.clientId)

                                )

                                select new
                                {


                                    chargeConsommerTotale= edp.ProjetLivraisons.SelectMany(x=>x.DetailLivraisons).SelectMany(x=>x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme)),


                                    chargePrprofile = (
                                                             from  tache in _context.Taches 
                                                             join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                                                             join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id      
                                                             join user in _context.Users on tache.UserId equals  user.IdUser
                                                             join Profile in _context.profileUser on user.IdUser equals Profile.userId
                                                             where Livraison.projetEdpId == edp.id && Profile.dateDebut>= edp.dateDebut && Profile.dateFin<= edp.dateFin 
                                                             group tache by Profile.profileId into g 

                                                          
                                                             
                                                             select new
                                                             {
                                                             
                                                              charge=   g.Sum(x=> Convert.ToDouble(x.chargeConsomme)) * Convert.ToDouble(_context.profile.Where(x=>x.annee== DateTime.Now.Year.ToString() && x.profileId==g.Key ).Select(x=>x.budget).FirstOrDefault())

                                                             }).Sum(x=>x.charge),

                    //Livraison.DetailLivraisons.SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme))).Distinct().Sum(),





                                    id = edp.id,
                                   commercantName = edp.Commercial.FullName,
                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                    natureCodeProjet = edp.CodeProjets.Nature,
                                    nom = edp.Nom,
                                     edp.status,
                                    budgetInitial = edp.BudgetInitial,
                                    budgetRallonge = edp.BudgetRallonge,
                                    clientName = edp.client.Nom,
                                    dateCreation = Convert.ToDateTime(edp.DateCreation),
                                    edp.budgetSenior,
                                    edp.budgetSeniorRallonge,

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
                                    ttm= edp.dateFinPrevue< DateTime.Now.Date && edp.ProjetLivraisons.Where(x=>x.StatusId!= "Delivered" && x.StatusId != "Canceled").Count()>0 ? "LATE" : (edp.dateFinPrevue > DateTime.Now.Date || edp.dateFinPrevue == null ? ""  : "ON TIME"),
                                    countProjetLivraison = edp.ProjetLivraisons.Count(),


                                }).Distinct().ToList().OrderBy(x=>x.clientName);


                return Ok(resultat);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
       
        }


        [HttpPost("satatDashboardLivraisonComtable")]
        public IActionResult satatDashboardLivraisonComtable(Search search)
        {
            try
            {
                
                if (search.dateDebut.Equals("1000-01-01 00:00:00") && search.dateFin.Equals("9999-01-01 23:59:59"))
                {

                    var dbresult = (from edp in _context.ProjetEdps
                                    where (
                                                  (search.idClients.Count == 0 || search.idClients.Contains(edp.clientId))
                                                  && (String.IsNullOrEmpty(search.commercialId) || search.commercialId == edp.CommercialId)
                                                  && (String.IsNullOrEmpty(search.status) || search.status == edp.status)
                                                     && (
                                                     ((string.IsNullOrEmpty(search.status) || search.status == "Running") && edp.status == "Running")

                                                                    || search.annees == edp.dateDebut.Value.Year
                                                                    || search.annees == edp.dateFin.Value.Year
                                                                    //|| search.annees == edp.dateFinInitial.Value.Year
                                                                    || search.annees == edp.dateFinPrevue.Value.Year
                                                                  //|| search.annees == Convert.ToInt32(edp.DateCreation.Substring(6, 4))
                                                                  )


                                                                  )
                                    join pl in _context.ProjetLivraisons on edp.id equals pl.projetEdpId
                                    into joinedT
                                    from r in joinedT.DefaultIfEmpty()

                                    join dl in _context.DetailLivraisons on r.Id equals dl.ProjetLivraisonId
                                       into joinedd
                                    from dd in joinedd.DefaultIfEmpty()

                                    join t in _context.Taches on dd.Id equals t.detailLivraisonId
                                                 into joinedt
                                    from tt in joinedt.DefaultIfEmpty()
                                    join p in _context.profileUser on tt.UserId equals p.userId
                                       into joinedtg
                                    from tte in joinedtg.DefaultIfEmpty()

                                    join di in _context.DetailImputations on tt.Id equals di.TacheId

                                      into joinedtge
                                    from tted in joinedtge.DefaultIfEmpty()
                               
                                    select new
                                    {
                                        budgetInitial = edp.BudgetInitial + (edp.BudgetRallonge != null ? edp.BudgetRallonge : 0),


                                        id = edp.id,
                                        commercantName = edp.Commercial.FullName,
                                        CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                        natureCodeProjet = edp.CodeProjets.Nature,
                                        nom = edp.Nom,
                                        clientName = edp.client.Nom,
                                        dateCreation = edp.DateCreation,
                                        edp.dateFin,
                                        edp.dateFinPrevue,
                                        edp.status,
                                        edp.Description,

                                        //budgetTotale = (edp.BudgetInitial ) + (edp.BudgetRallonge != null ? edp.BudgetRallonge : 0),
                                        budgetConfirmeTotale = (edp.budgetConfirmeRallonge != null ? edp.budgetConfirmeRallonge : 0) + (edp.budgetConfirme != null ? edp.budgetConfirme : 0),

                                        budgetGPTotale = (edp.budgetGPRallonge != null ? edp.budgetGPRallonge : 0) + (edp.budgetGP != null ? edp.budgetGP : 0),


                                        budgetJuniorTotale = (edp.budgetJunior != null ? edp.budgetJunior : 0) + (edp.budgetJuniorRallonge != null ? edp.budgetJuniorRallonge : 0),

                                        budgetValidationTotale = (edp.budgetValidation != null ? edp.budgetValidation : 0) + (edp.budgetValidationRallonge != null ? edp.budgetValidationRallonge : 0),

                                        budgetSeniorTotale = (edp.budgetSenior != null ? edp.budgetSenior : 0) + (edp.budgetSeniorRallonge != null ? edp.budgetSeniorRallonge : 0),

                                        budgetDirectionTotale = (edp.budgetDirection != null ? edp.budgetDirection : 0) + (edp.budgetDirectionRallonge != null ? edp.budgetDirectionRallonge : 0),


                                        countProjetLivraison = edp.ProjetLivraisons.Count(),

                                        tte.profileId,
                                        Monday = (Convert.ToDouble(tted.Monday.Substring(0, 2)) + Convert.ToDouble(tted.Monday.Substring(3, 2)) / 60) / 8,
                                        Tuesday = (Convert.ToDouble(tted.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted.Tuesday.Substring(3, 2)) / 60) / 8,
                                        Friday = (Convert.ToDouble(tted.Friday.Substring(0, 2)) + Convert.ToDouble(tted.Friday.Substring(3, 2)) / 60) / 8,
                                        Wednesday = (Convert.ToDouble(tted.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted.Wednesday.Substring(3, 2)) / 60) / 8,
                                        Thursday = (Convert.ToDouble(tted.Thursday.Substring(0, 2)) + Convert.ToDouble(tted.Thursday.Substring(3, 2)) / 60) / 8,
                                        //                    Monday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Monday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Monday.Substring(3, 2))) / 60) / 8,
                                        //                    Tuesday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Tuesday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Tuesday.Substring(3, 2))) / 60) / 8,
                                        //                    Friday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Friday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Friday.Substring(3, 2))) / 60) / 8,
                                        //                    Wednesday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Wednesday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Wednesday.Substring(3, 2))) / 60) / 8,
                                        //                    Thursday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Thursday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Thursday.Substring(3, 2))) / 60) / 8,

                                    }).ToList();
                    var result1 = (from edp in dbresult
                                   group edp by new
                                   {
                                       edp.profileId,
                                       edp.Description,
                                       edp.id,
                                       edp.commercantName,
                                       edp.CodeProjet,
                                       edp.natureCodeProjet,
                                       edp.nom,
                                       edp.budgetInitial,
                                       edp.clientName,
                                       edp.dateCreation,
                                       edp.dateFin,
                                       edp.dateFinPrevue,
                                       edp.status,
                                       edp.budgetDirectionTotale,
                                       edp.budgetConfirmeTotale,
                                       edp.budgetGPTotale,
                                       edp.budgetJuniorTotale,
                                       edp.budgetValidationTotale,
                                       edp.budgetSeniorTotale,
                                       //edp.budgetTotale,
                                       edp.countProjetLivraison,
                                   } into g
                                   select new
                                   {
                                       g.Key.profileId,
                                       //g.Key.budgetTotale,
                                       g.Key.id,
                                       g.Key.Description,
                                       g.Key.commercantName,
                                       g.Key.dateFinPrevue,
                                       g.Key.dateFin,
                                       g.Key.CodeProjet,
                                       g.Key.natureCodeProjet,
                                       g.Key.nom,
                                       g.Key.budgetInitial,
                                       g.Key.clientName,
                                       g.Key.dateCreation,
                                       g.Key.status,
                                       g.Key.countProjetLivraison,




                                       coutEstime = g.Key.budgetConfirmeTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "confirmé").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetDirectionTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Direction").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetGPTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "GP").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetJuniorTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "junior").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetSeniorTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "sénior").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetValidationTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Validation").Select(x => x.budget).FirstOrDefault()),

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
                                           edp.Description,

                                           edp.dateFin,
                                           edp.dateFinPrevue,
                                           edp.id,
                                           edp.commercantName,
                                           edp.CodeProjet,
                                           edp.natureCodeProjet,
                                           edp.nom,
                                           edp.budgetInitial,
                                           edp.clientName,
                                           edp.dateCreation,

                                           edp.status,

                                           //edp.budgetTotale,



                                           edp.countProjetLivraison,

                                           edp.coutEstime

                                       } into g
                                       select new
                                       {




                                           g.Key.id,
                                           g.Key.Description,
                                           g.Key.commercantName,
                                           g.Key.CodeProjet,
                                           g.Key.natureCodeProjet,
                                           g.Key.dateFin,
                                           g.Key.dateFinPrevue,
                                           g.Key.nom,
                                           g.Key.clientName,
                                           g.Key.dateCreation,

                                           g.Key.status,
                                           g.Key.countProjetLivraison,

                                           g.Key.budgetInitial,
                                           showDetails = false,
                                           coutEstime = g.Key.coutEstime == 0 ? g.Key.budgetInitial * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Estimation").Select(x => x.budget).FirstOrDefault()) : g.Key.coutEstime,
                                           estimation = g.Key.coutEstime == 0 ? true : false,
                                           charge = g.Sum(x => x.charge),
                                           cout = g.Sum(x => x.cout)




                                       }).ToList();



                    var ResultGroupByClient = (from edp in ResultFinal
                                               group edp by new
                                               {

                                                   edp.clientName,




                                               } into g
                                               select new
                                               {




                                                   g.Key.clientName,
                                                   rowSpan = g.Count(),
                                                   maxdate = g.Select(x => x.dateFinPrevue).Max(),
                                                   listProjet = g.ToList().OrderBy(x => x.dateFinPrevue),



                                                   //    countProjetLivraison= g.Sum(x=>x.countProjetLivraison)
                                               }).ToList().OrderByDescending(x => x.maxdate);


                    return Ok(ResultGroupByClient);


                }
                else
                {
                    var dbresult = (from edp in _context.ProjetEdps
                                    where (
                                                  (search.idClients.Count == 0 || search.idClients.Contains(edp.clientId))
                                                  && (String.IsNullOrEmpty(search.commercialId) || search.commercialId == edp.CommercialId)
                                                  )
                                    join pl in _context.ProjetLivraisons on edp.id equals pl.projetEdpId
                                    into joinedT
                                    from r in joinedT

                                    join dl in _context.DetailLivraisons on r.Id equals dl.ProjetLivraisonId
                                       into joinedd
                                    from dd in joinedd

                                    join t in _context.Taches on dd.Id equals t.detailLivraisonId
                                                 into joinedt
                                    from tt in joinedt
                                    join p in _context.profileUser on tt.UserId equals p.userId
                                       into joinedtg
                                    from tte in joinedtg

                                    join di in _context.DetailImputations on tt.Id equals di.TacheId

                                      into joinedtge
                                    from tted in joinedtge
                                    join imputa in _context.Imputations on tted.ImputationId equals imputa.Id
                                    into NewFiltre
                                    from rc in NewFiltre
                                    where ((
                                          (   (rc.mondayDate.Value.Date < Convert.ToDateTime(search.dateFin) && rc.mondayDate.Value.Date > Convert.ToDateTime(search.dateDebut) && rc.DetailImputations.Select(e => e.Monday).Any(e => e != "00:00")) )

                                          )

                                           ||
                                           ((rc.TuesdayDate.Value.Date < Convert.ToDateTime(search.dateFin) &&  rc.TuesdayDate.Value.Date > Convert.ToDateTime(search.dateDebut)) && rc.DetailImputations.Select(e => e.Tuesday).Any(e => e != "00:00") )

                                           
                                           ||
                                           ((rc.WednesdayDate.Value.Date < Convert.ToDateTime(search.dateFin) && rc.WednesdayDate.Value.Date > Convert.ToDateTime(search.dateDebut)) && rc.DetailImputations.Select(e => e.Wednesday).Any(e => e != "00:00") 

                                           )
                                              ||
                                           ( (rc.ThursdayDate.Value.Date < Convert.ToDateTime(search.dateFin) && rc.ThursdayDate.Value.Date > Convert.ToDateTime(search.dateDebut)) && rc.DetailImputations.Select(e => e.Thursday).Any(e => e != "00:00") 

                                           )
                                           ||
                                           ((rc.FridayDate.Value.Date < Convert.ToDateTime(search.dateFin) && rc.FridayDate.Value.Date > Convert.ToDateTime(search.dateDebut)) && rc.DetailImputations.Select(e => e.Friday).Any(e => e != "00:00")

                                            )



                                           )
                                    select new
                                    {
                                        budgetInitial = edp.BudgetInitial + (edp.BudgetRallonge != null ? edp.BudgetRallonge : 0),


                                        id = edp.id,
                                        commercantName = edp.Commercial.FullName,
                                        CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                        natureCodeProjet = edp.CodeProjets.Nature,
                                        nom = edp.Nom,
                                        clientName = edp.client.Nom,
                                        dateCreation = edp.DateCreation,
                                        edp.dateFin,
                                        edp.dateFinPrevue,
                                        edp.status,
                                        edp.Description,

                                        //budgetTotale = (edp.BudgetInitial ) + (edp.BudgetRallonge != null ? edp.BudgetRallonge : 0),
                                        budgetConfirmeTotale = (edp.budgetConfirmeRallonge != null ? edp.budgetConfirmeRallonge : 0) + (edp.budgetConfirme != null ? edp.budgetConfirme : 0),

                                        budgetGPTotale = (edp.budgetGPRallonge != null ? edp.budgetGPRallonge : 0) + (edp.budgetGP != null ? edp.budgetGP : 0),


                                        budgetJuniorTotale = (edp.budgetJunior != null ? edp.budgetJunior : 0) + (edp.budgetJuniorRallonge != null ? edp.budgetJuniorRallonge : 0),

                                        budgetValidationTotale = (edp.budgetValidation != null ? edp.budgetValidation : 0) + (edp.budgetValidationRallonge != null ? edp.budgetValidationRallonge : 0),

                                        budgetSeniorTotale = (edp.budgetSenior != null ? edp.budgetSenior : 0) + (edp.budgetSeniorRallonge != null ? edp.budgetSeniorRallonge : 0),

                                        budgetDirectionTotale = (edp.budgetDirection != null ? edp.budgetDirection : 0) + (edp.budgetDirectionRallonge != null ? edp.budgetDirectionRallonge : 0),


                                        countProjetLivraison = edp.ProjetLivraisons.Count(),

                                        tte.profileId,
                                        Monday = (Convert.ToDouble(tted.Monday.Substring(0, 2)) + Convert.ToDouble(tted.Monday.Substring(3, 2)) / 60) / 8,
                                        Tuesday = (Convert.ToDouble(tted.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted.Tuesday.Substring(3, 2)) / 60) / 8,
                                        Friday = (Convert.ToDouble(tted.Friday.Substring(0, 2)) + Convert.ToDouble(tted.Friday.Substring(3, 2)) / 60) / 8,
                                        Wednesday = (Convert.ToDouble(tted.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted.Wednesday.Substring(3, 2)) / 60) / 8,
                                        Thursday = (Convert.ToDouble(tted.Thursday.Substring(0, 2)) + Convert.ToDouble(tted.Thursday.Substring(3, 2)) / 60) / 8,
                                        date = rc.FridayDate.Value.Date
                                        //                    Monday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Monday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Monday.Substring(3, 2))) / 60) / 8,
                                        //                    Tuesday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Tuesday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Tuesday.Substring(3, 2))) / 60) / 8,
                                        //                    Friday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Friday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Friday.Substring(3, 2))) / 60) / 8,
                                        //                    Wednesday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Wednesday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Wednesday.Substring(3, 2))) / 60) / 8,
                                        //                    Thursday = (Convert.ToDouble(rc.DetailImputations.Select(e => e.Thursday.Substring(0, 2))) + Convert.ToDouble(rc.DetailImputations.Select(e => e.Thursday.Substring(3, 2))) / 60) / 8,

                                    }).ToList();
                    var result1 = (from edp in dbresult
                                   group edp by new
                                   {
                                       edp.profileId,
                                       edp.Description,
                                       edp.id,
                                       edp.commercantName,
                                       edp.CodeProjet,
                                       edp.natureCodeProjet,
                                       edp.nom,
                                       edp.budgetInitial,
                                       edp.clientName,
                                       edp.dateCreation,
                                       edp.dateFin,
                                       edp.dateFinPrevue,
                                       edp.status,
                                       edp.budgetDirectionTotale,
                                       edp.budgetConfirmeTotale,
                                       edp.budgetGPTotale,
                                       edp.budgetJuniorTotale,
                                       edp.budgetValidationTotale,
                                       edp.budgetSeniorTotale,
                                       //edp.budgetTotale,
                                       edp.countProjetLivraison,
                                   } into g
                                   select new
                                   {
                                       g.Key.profileId,
                                       //g.Key.budgetTotale,
                                       g.Key.id,
                                       g.Key.Description,
                                       g.Key.commercantName,
                                       g.Key.dateFinPrevue,
                                       g.Key.dateFin,
                                       g.Key.CodeProjet,
                                       g.Key.natureCodeProjet,
                                       g.Key.nom,
                                       g.Key.budgetInitial,
                                       g.Key.clientName,
                                       g.Key.dateCreation,
                                       g.Key.status,
                                       g.Key.countProjetLivraison,




                                       coutEstime = g.Key.budgetConfirmeTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "confirmé").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetDirectionTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Direction").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetGPTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "GP").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetJuniorTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "junior").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetSeniorTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "sénior").Select(x => x.budget).FirstOrDefault())
                                      + g.Key.budgetValidationTotale * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Validation").Select(x => x.budget).FirstOrDefault()),

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
                                           edp.Description,

                                           edp.dateFin,
                                           edp.dateFinPrevue,
                                           edp.id,
                                           edp.commercantName,
                                           edp.CodeProjet,
                                           edp.natureCodeProjet,
                                           edp.nom,
                                           edp.budgetInitial,
                                           edp.clientName,
                                           edp.dateCreation,

                                           edp.status,

                                           //edp.budgetTotale,



                                           edp.countProjetLivraison,

                                           edp.coutEstime

                                       } into g
                                       select new
                                       {




                                           g.Key.id,
                                           g.Key.Description,
                                           g.Key.commercantName,
                                           g.Key.CodeProjet,
                                           g.Key.natureCodeProjet,
                                           g.Key.dateFin,
                                           g.Key.dateFinPrevue,
                                           g.Key.nom,
                                           g.Key.clientName,
                                           g.Key.dateCreation,

                                           g.Key.status,
                                           g.Key.countProjetLivraison,

                                           g.Key.budgetInitial,
                                           showDetails = false,
                                           coutEstime = g.Key.coutEstime == 0 ? g.Key.budgetInitial * Convert.ToDouble(_context.profile.Where(x => x.profileId == "Estimation").Select(x => x.budget).FirstOrDefault()) : g.Key.coutEstime,
                                           estimation = g.Key.coutEstime == 0 ? true : false,
                                           charge = g.Sum(x => x.charge),
                                           cout = g.Sum(x => x.cout)




                                       }).ToList();



                    var ResultGroupByClient = (from edp in ResultFinal
                                               group edp by new
                                               {

                                                   edp.clientName,




                                               } into g
                                               select new
                                               {




                                                   g.Key.clientName,
                                                   rowSpan = g.Count(),
                                                   maxdate = g.Select(x => x.dateFinPrevue).Max(),
                                                   listProjet = g.ToList().OrderBy(x => x.dateFinPrevue),



                                                   //    countProjetLivraison= g.Sum(x=>x.countProjetLivraison)
                                               }).ToList().OrderByDescending(x => x.maxdate);


                    return Ok(ResultGroupByClient);
                }




          

            } catch (Exception e)
            {

                return BadRequest(e);
            }


  

        }

        [HttpPost("satatByClientByComercial")]
        public IActionResult satatByClientByComercial(Search search)
        {
            try
            {


                var dbresult = (from  edp in _context.ProjetEdps
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
                                join pl in _context.ProjetLivraisons on edp.id equals pl.projetEdpId
                                into joinedT
                                from r in joinedT.DefaultIfEmpty()

                                join dl in _context.DetailLivraisons on r.Id equals dl.ProjetLivraisonId
                                   into joinedd
                                from dd in joinedd.DefaultIfEmpty()

                                join t in _context.Taches on dd.Id equals t.detailLivraisonId
                                             into joinedt
                                from tt in joinedt.DefaultIfEmpty()
                                join p in _context.profileUser on tt.UserId equals p.userId
                                   into joinedtg
                                from tte in joinedtg.DefaultIfEmpty()

                                join  di in _context.DetailImputations on tt.Id equals di.TacheId

                                  into joinedtge
                                from tted in joinedtge.DefaultIfEmpty()



                                select new
                                {
                                    budgetInitial = edp.BudgetInitial + (edp.BudgetRallonge!=null? edp.BudgetRallonge :0),


                                    id = edp.id,
                                    commercantName = edp.Commercial.FullName,
                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                    natureCodeProjet = edp.CodeProjets.Nature,
                                    nom = edp.Nom,
                                    clientName = edp.client.Nom,
                                    dateCreation = edp.DateCreation,

                                    edp.status,


                                    budgetConfirmeTotale = (edp.budgetConfirmeRallonge != null ? edp.budgetConfirmeRallonge : 0)+ (edp.budgetConfirme != null ? edp.budgetConfirme : 0),

                                    budgetGPTotale = (edp.budgetGPRallonge != null ? edp.budgetGPRallonge : 0) + (edp.budgetGP != null ? edp.budgetGP : 0),

                          
                                    budgetJuniorTotale = (edp.budgetJunior != null ? edp.budgetJunior : 0) + (edp.budgetJuniorRallonge != null ? edp.budgetJuniorRallonge : 0),
                       
                                    budgetValidationTotale = (edp.budgetValidation != null ? edp.budgetValidation : 0) + (edp.budgetValidationRallonge != null ? edp.budgetValidationRallonge : 0),
                            
                                    budgetSeniorTotale = (edp.budgetSenior != null ? edp.budgetSenior : 0) + (edp.budgetSeniorRallonge != null ? edp.budgetSeniorRallonge : 0),
                        
                                    budgetDirectionTotale = (edp.budgetDirection != null ? edp.budgetDirection : 0) + (edp.budgetDirectionRallonge != null ? edp.budgetDirectionRallonge : 0),
                         

                                    countProjetLivraison = edp.ProjetLivraisons.Count(),

                                    tte.profileId,
                                   Monday = (Convert.ToDouble(tted.Monday.Substring(0, 2)) + Convert.ToDouble(tted.Monday.Substring(3, 2)) / 60)/8,
                                    Tuesday = (Convert.ToDouble(tted.Tuesday.Substring(0, 2)) + Convert.ToDouble(tted.Tuesday.Substring(3, 2)) / 60) / 8,
                                    Friday = (Convert.ToDouble(tted.Friday.Substring(0, 2)) + Convert.ToDouble(tted.Friday.Substring(3, 2)) / 60 )/ 8,
                                    Wednesday = (Convert.ToDouble(tted.Wednesday.Substring(0, 2)) + Convert.ToDouble(tted.Wednesday.Substring(3, 2)) / 60) / 8,
                                    Thursday = (Convert.ToDouble(tted.Thursday.Substring(0, 2)) + Convert.ToDouble(tted.Thursday.Substring(3, 2)) / 60) / 8,
                        

                                }).ToList();




                var result1 = (from edp in dbresult
                               group edp by new
                               {
                                   edp.profileId,


                                   edp.id,
                                   edp.commercantName,
                                   edp.CodeProjet,
                                   edp.natureCodeProjet,
                                   edp.nom,
                                   edp.budgetInitial,
                                   edp.clientName,
                                   edp.dateCreation,

                                   edp.status,

                                   edp.budgetDirectionTotale,
                                   edp.budgetConfirmeTotale,
                                   edp.budgetGPTotale,
                                   edp.budgetJuniorTotale,
                                   edp.budgetValidationTotale,
                                   edp.budgetSeniorTotale,


                                   edp.countProjetLivraison,



                               } into g
                               select new
                               {


                                   g.Key.profileId,
                                   g.Key.id,
                                   g.Key.commercantName,
                                   g.Key.CodeProjet,
                                   g.Key.natureCodeProjet,
                                   g.Key.nom,
                                   g.Key.budgetInitial,
                                   g.Key.clientName,
                                   g.Key.dateCreation,

                                   g.Key.status,
                                   g.Key.countProjetLivraison,
                                   coutEstime = (g.Key.profileId == "confirmé" ? g.Key.budgetConfirmeTotale
                    : (g.Key.profileId == "Direction" ? g.Key.budgetDirectionTotale
                    : (g.Key.profileId == "GP" ? g.Key.budgetGPTotale
                    : (g.Key.profileId == "junior" ? g.Key.budgetJuniorTotale
                    : (g.Key.profileId == "sénior" ? g.Key.budgetSeniorTotale
                    : (g.Key.profileId == "Validation" ? g.Key.budgetValidationTotale
                   : 0)))))
                    ) * Convert.ToDouble(_context.profile.Where(x => x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault()),


                                   charge = g.Sum(x => x.Monday) + g.Sum(x => x.Tuesday) + g.Sum(x => x.Wednesday) + g.Sum(x => x.Friday) + g.Sum(x => x.Thursday),
                     
                                   cout = (g.Sum(x => x.Monday) +g.Sum(x => x.Tuesday) +g.Sum(x => x.Wednesday) +g.Sum(x => x.Friday) +g.Sum(x => x.Thursday)) 
                                            
                                  * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault())
                               }).ToList();



                var ResultFinal = (from edp in result1
                                   group edp by new
                                   {


                                       edp.id,
                                       edp.commercantName,
                                       edp.CodeProjet,
                                       edp.natureCodeProjet,
                                       edp.nom,
                                       edp.budgetInitial,
                                       edp.clientName,
                                       edp.dateCreation,

                                       edp.status,




                                       edp.countProjetLivraison,



                                   } into g
                                   select new
                                   {




                                       g.Key.id,
                                       g.Key.commercantName,
                                       g.Key.CodeProjet,
                                       g.Key.natureCodeProjet,
                                       g.Key.nom,
                                       g.Key.clientName,
                                       g.Key.dateCreation,

                                       g.Key.status,
                                       g.Key.countProjetLivraison,

                                       g.Key.budgetInitial,

                                       coutEstime = g.Sum(x => x.coutEstime),
                                       charge = g.Sum(x => x.charge),
                                       cout = g.Sum(x => x.cout)




                                   }).ToList();


                return Ok(ResultFinal);
                //                var coutConsomme = (
                //                                               from de in _context.DetailImputations



                //                                               join tache in _context.Taches on de.TacheId equals tache.Id
                //                                               join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                //                                               join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
                //                                               join user in _context.Users on tache.UserId equals user.IdUser
                //                                               join Profile in _context.profileUser on user.IdUser equals Profile.userId
                //                                         //          where Livraison.projetEdpId == edp.id /*&& Profile.dateDebut >= edp.dateDebut  && (Profile.dateFin <= edp.dateFin || edp.dateFin == null)*/
                //                                         group de by new {Profile.profileId, Livraison.projetEdpId } into g



                //                                         select new resultat
                //                                         {



                //                                             item = "1",
                //                                             charge = (double)(g.Sum(x => Convert.ToDouble(x.Monday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Monday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Thursday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Friday.Substring(3, 2))) / 60)
                //          ,
                //                                             cout = ((double)(g.Sum(x => Convert.ToDouble(x.Monday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Monday.Substring(3, 2))) / 60)
                //          + (double)(g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(3, 2))) / 60)
                //          + (double)(g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(3, 2))) / 60)
                //          + (double)(g.Sum(x => Convert.ToDouble(x.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Thursday.Substring(3, 2))) / 60)
                //          + (double)(g.Sum(x => Convert.ToDouble(x.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Friday.Substring(3, 2))) / 60)) * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key.profileId).Select(x => x.budget).FirstOrDefault()),


                //                                         });





                //                var resultat = (from edp in _context.ProjetEdps
                //                                where(
                //                                (search.idClients.Count == 0 || search.idClients.Contains(edp.clientId))
                //                                && (String.IsNullOrEmpty(search.commercialId)|| search.commercialId==edp.CommercialId)
                //                                && (String.IsNullOrEmpty(search.status) || search.status == edp.status)
                //                                   && (
                //                                   ((string.IsNullOrEmpty(search.status) || search.status == "Running") && edp.status == "Running")

                //                                || search.annees == edp.dateDebut.Value.Year
                //                                || search.annees == edp.dateFin.Value.Year
                //                                || search.annees == edp.dateFinInitial.Value.Year
                //                                || search.annees == edp.dateFinPrevue.Value.Year
                //                                || search.annees == Convert.ToInt32(edp.DateCreation.Substring(6, 4))
                //                                ))

                //                                select new
                //                                {


                //                                    //chargeConsommerTotale = edp.ProjetLivraisons.SelectMany(x => x.DetailLivraisons).SelectMany(x => x.taches).Select(x=>x.DetailImputations).Select(x=>new { }),


                //                                    coutConsomme = (
                //                                                             from de in _context.DetailImputations



                //                                                             join tache in _context.Taches on de.TacheId equals tache.Id
                //                                                             join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                //                                                             join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
                //                                                             join user in _context.Users on tache.UserId equals user.IdUser
                //                                                             join Profile in _context.profileUser on user.IdUser equals Profile.userId
                //                                                             where Livraison.projetEdpId == edp.id
                //                                                             /*&& Profile.dateDebut >= edp.dateDebut  && (Profile.dateFin <= edp.dateFin || edp.dateFin == null)*/
                //                                                             select new {
                //                                                                 Profile.profileId,

                //                                                                 de.Monday,
                //                                                                 de.Tuesday,
                //                                                                 de.Thursday,
                //                                                                 de.Friday,
                //                                                                 de.Wednesday,
                //                                                             }).T.GroupBy(x => x.profileId).Select(g => new
                //                                                             {

                //                                                                 item = "1",
                //                                                                 charge = (double)(g.Sum(x => Convert.ToDouble(x.Monday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Monday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Thursday.Substring(3, 2))) / 60)
                //+ (double)(g.Sum(x => Convert.ToDouble(x.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Friday.Substring(3, 2))) / 60)
                //                              ,
                //                                                                 cout = ((double)(g.Sum(x => Convert.ToDouble(x.Monday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Monday.Substring(3, 2))) / 60)
                //                              + (double)(g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Tuesday.Substring(3, 2))) / 60)
                //                              + (double)(g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Wednesday.Substring(3, 2))) / 60)
                //                              + (double)(g.Sum(x => Convert.ToDouble(x.Thursday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Thursday.Substring(3, 2))) / 60)
                //                              + (double)(g.Sum(x => Convert.ToDouble(x.Friday.Substring(0, 2))) + g.Sum(x => Convert.ToDouble(x.Friday.Substring(3, 2))) / 60)) * Convert.ToDouble(_context.profile.Where(x => x.annee == DateTime.Now.Year.ToString() && x.profileId == g.Key).Select(x => x.budget).FirstOrDefault()),

                //                                                             }).ToList(),




                //                id = edp.id,
                //                                      commercantName = edp.Commercial.FullName,
                //                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                //                                    natureCodeProjet = edp.CodeProjets.Nature,
                //                                    nom = edp.Nom,
                //                                    budgetInitial = edp.BudgetInitial,
                //                                    budgetRallonge = edp.BudgetRallonge,
                //                                    clientName = edp.client.Nom,
                //                                    dateCreation = Convert.ToDateTime(edp.DateCreation),
                //                                    edp.budgetSenior,
                //                                    edp.budgetSeniorRallonge,
                //                                    edp.status,

                //                                    edp.budgetConfirme,
                //                                    edp.budgetDirection,
                //                                    edp.budgetGP,
                //                                    edp.budgetJunior,
                //                                    edp.budgetValidation,
                //                                    budgetConfirmeTotale = edp.budgetConfirmeRallonge + edp.budgetConfirme,
                //                                    budgetGPTotale = edp.budgetGPRallonge + edp.budgetGP,
                //                                    budgetJuniorTotale = edp.budgetJuniorRallonge + edp.budgetJunior,
                //                                    budgetValidationTotale = edp.budgetValidationRallonge + edp.budgetValidation,
                //                                    budgetSeniorTotale = edp.budgetSeniorRallonge + edp.budgetSenior,

                //                                    edp.budgetConfirmeRallonge,
                //                                    edp.budgetDirectionRallonge,
                //                                    edp.budgetGPRallonge,

                //                                    edp.budgetJuniorRallonge,
                //                                    edp.budgetValidationRallonge,
                //                                    countProjetLivraison = edp.ProjetLivraisons.Count(),


                //                                }).Distinct().ToList();



            }
            catch (Exception e)
            {

                return BadRequest(e);
            }

        }


        [HttpGet("stat")]
        public IActionResult StatProjetCompatble()
        {
            try { 
            var resultat = (from edp in _context.ProjetEdps select new
                            {
                chargeConsommerTotale = (from impu in _context.Imputations
                                         join Dimpu in _context.DetailImputations on impu.Id equals Dimpu.ImputationId
                                         join tache in _context.Taches on Dimpu.TacheId equals tache.Id
                                         join DLivraison in _context.DetailLivraisons on tache.detailLivraisonId equals DLivraison.Id
                                         join Livraison in _context.ProjetLivraisons on DLivraison.ProjetLivraisonId equals Livraison.Id
                                         where Livraison.projetEdpId == edp.id && impu.StatusImputation == 3
                                         select
Livraison.DetailLivraisons.SelectMany(x => x.taches).Sum(x => Convert.ToDouble(x.chargeConsomme))).Distinct().Sum(),





                id = edp.id,
                commercantName = edp.Commercial.FullName,
                CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                natureCodeProjet = edp.CodeProjets.Nature,
                nom = edp.Nom,
                                budgetInitial = edp.BudgetInitial,
                                budgetRallonge = edp.BudgetRallonge,
                                clientName = edp.client.Nom,
                                dateCreation= Convert.ToDateTime(edp.DateCreation),

                countProjetLivraison = edp.ProjetLivraisons.Count(),
                                //   projetLivraisonName = PL.ProjetName,
                                //chargeConsommerTotale = dpl.taches.Count() > 0 ? dpl.taches.Sum(x => Convert.ToDouble(x.chargeConsomme)) : 0,
      



                                
                       
                            }).Distinct().ToList();

            //var resultat = (from PL in _context.ProjetLivraisons
            //               join edp in _context.ProjetEdps on PL.projetEdpId equals edp.id
            //                join dpl in _context.DetailLivraisons on PL.Id equals dpl.ProjetLivraisonId

            //                select new
            //                {
            //                    nom = edp.Nom,
            //                    budgetInitial = edp.BudgetInitial,
            //                    budgetRallonge = edp.BudgetRallonge,
            //                    clientName = edp.client.Nom,
            //                    projetLivraisonName= PL.ProjetName,
            //                    chargeConsommerTotale= dpl.taches.Count()>0 ?dpl.taches.Sum(x => Convert.ToDouble(x.chargeConsomme)) : 0,




            //                    countProjetLivraison = edp.ProjetLivraisons.Count()
            //                }).ToList();

            return Ok(resultat);
        }
            catch (Exception e)
            {

                return BadRequest(e);
    }
}


        
       [HttpPost("getByCommercial")]
        public IActionResult getByCommercial(FiltreProjetComptableDto model)
        {
            try
            {
                var resultat = (from edp in _context.ProjetEdps
                                where (edp.CommercialId == model.commercialId &&
                                (string.IsNullOrEmpty(model.client) || model.client == edp.clientId)
                                && (string.IsNullOrEmpty(model.statut) || model.statut == edp.status)
                                && (((string.IsNullOrEmpty(model.statut) || model.statut == "Running") && edp.status == "Running")

                                || model.annees == edp.dateDebut.Value.Year
                                || model.annees == edp.dateFin.Value.Year
                                || model.annees == edp.dateFinInitial.Value.Year
                                || model.annees == edp.dateFinPrevue.Value.Year
                                || model.annees == Convert.ToInt32(edp.DateCreation.Substring(6, 4))
                                ))
              
                                select new
                                {
                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                    natureCodeProjet = edp.CodeProjets.Nature,
                                    edp.dateDebut,
                                    edp.dateFin,
                                    edp.status,
                                    edp.DateCreation,
                                    edp.dateFinInitial,
                                    id = edp.id,
                                    nom = edp.Nom,
                                    budgetInitial = edp.BudgetInitial,
                                    budgetRallonge = edp.BudgetRallonge,
                                    edp.Description,
                                    clientName = edp.client.Nom,
                                    clientId = edp.clientId,
                                    commercantName = edp.Commercial.FullName,
                                    countProjetLivraison = edp.ProjetLivraisons.Count()
                                }).ToList().OrderBy(x => Convert.ToDateTime(x.DateCreation)).ThenBy(x => x.clientName);

                return Ok(resultat);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }
        // GET: api/ProjetEdps
        [HttpPost("filtre")]
        public IActionResult GetProjetEdps(FiltreProjetComptableDto model)
        {
            try
            {
                var resultat = (from edp in _context.ProjetEdps
                                where (
                                (string.IsNullOrEmpty(model.client) || model.client == edp.clientId)
                                && (string.IsNullOrEmpty(model.statut) || model.statut == edp.status)
                                && (
                                 ((string.IsNullOrEmpty(model.statut) || model.statut == "Running") && edp.status == "Running")

                                || model.annees == edp.dateDebut.Value.Year
                                || model.annees == edp.dateFin.Value.Year
                                || model.annees == edp.dateFinInitial.Value.Year
                                || model.annees == edp.dateFinPrevue.Value.Year
                                || model.annees == Convert.ToInt32(edp.DateCreation.Substring(6,4)
                           
                                )
                                )
                )
                                select new
                                {
                                    CodeProjet = edp.CodeProjets.Numero + "_" + edp.CodeProjet,
                                    natureCodeProjet = edp.CodeProjets.Nature,
                                    edp.dateDebut,
                                    edp.dateFin,
                                    edp.status,
                                    edp.dateFinInitial,
                                    edp.dateFinPrevue,
                                    edp.DateCreation,
                                    id = edp.id,
                                    nom = edp.Nom,
                                    budgetInitial = edp.BudgetInitial,
                                    budgetRallonge = edp.BudgetRallonge,
                                    edp.Description,
                                    clientName = edp.client.Nom,
                                    clientId = edp.clientId,
                                    commercantName = edp.Commercial.FullName,
                                    countProjetLivraison = edp.ProjetLivraisons.Count()
                                }).ToList().OrderBy(x =>Convert.ToDateTime(x.DateCreation)).ThenBy(x =>x.clientName);

                return Ok(resultat);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }
        // GET: api/ProjetEdps
        [HttpGet]
        public  IActionResult GetProjetEdps()
        {

    try { 
            var resultat=  (from edp in  _context.ProjetEdps
                select new
            {
                    CodeProjet =edp.CodeProjets.Numero+"_"+ edp.CodeProjet,
                    natureCodeProjet = edp.CodeProjets.Nature,
edp.dateDebut,
edp.dateFin,
edp.status,
edp.dateFinInitial,
                    edp.dateFinPrevue,

                    id = edp.id,
                nom = edp.Nom,
                    budgetInitial = edp.BudgetInitial,
                    budgetRallonge = edp.BudgetRallonge,
                    edp.Description,
                    clientName = edp.client.Nom,
                    clientId = edp.clientId,
                    commercantName=edp.Commercial.FullName,
                    countProjetLivraison = edp.ProjetLivraisons.Count()
            }).ToList().OrderBy(x=>x.clientName);

            return Ok(resultat);
}
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }


                // GET: api/ProjetEdps
        [HttpGet("running")]
        public  IActionResult GetRunningProjet()
        {

    try { 
            var resultat=  (from edp in  _context.ProjetEdps where edp.status== "Running"
                            select new
            {
edp.status,

                nom = edp.Nom,
                                id = edp.id,
                    clientId = edp.clientId,
                    commercantName=edp.Commercial.FullName,
            }).ToList().OrderBy(x=>x.nom);

            return Ok(resultat);
}
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }


        // GET: api/ProjetEdps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetEdp>> GetProjetEdp(string id)
{
    try { 
            var projetEdp = await _context.ProjetEdps.FindAsync(id);

            if (projetEdp == null)
            {
                return NotFound();
            }

            return projetEdp;
}
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }

        // PUT: api/ProjetEdps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PutProjetEdp(string id, ProjetEdp projetEdp)
        {
            if (id != projetEdp.id)
            {
                return BadRequest();
            }

            var projetEdp1 = await _context.ProjetEdps.Where(x => x.CodeProjet == projetEdp.CodeProjet && x.CodeProjetsId == projetEdp.CodeProjetsId && x.id!= projetEdp.id).FirstOrDefaultAsync();

            if (projetEdp1 != null)
            {
                return Ok(new { codeRetour = 500 });
            }

            _context.Entry(projetEdp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { codeRetour = 200 });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjetEdpExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/ProjetEdps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        
        [HttpPost]
        public async Task<ActionResult<ProjetEdp>> PostProjetEdp(ProjetEdp projetEdp)
        {

            var projetEdp1 = await _context.ProjetEdps.Where(x=>x.CodeProjet==projetEdp.CodeProjet && x.CodeProjetsId== projetEdp.CodeProjetsId).FirstOrDefaultAsync();

            if (projetEdp1!=null)
            {
                return Ok(new { codeRetour = 500 });
            }
            projetEdp.DateCreation = DateTime.Now.ToString();

            _context.ProjetEdps.Add(projetEdp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjetEdp", new { id = projetEdp.id, codeRetour = 200 }, projetEdp);
        }


        // DELETE: api/ProjetEdps/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<ProjetEdp>> DeleteProjetEdp(string id)
        {
            var projetEdp = await _context.ProjetEdps.FindAsync(id);
            if (projetEdp == null)
            {
                return NotFound();
            }

            _context.ProjetEdps.Remove(projetEdp);
            await _context.SaveChangesAsync();

            return projetEdp;
        }


        [NonAction]
        private bool ProjetEdpExists(string id)
        {
            return _context.ProjetEdps.Any(e => e.id == id);
        }
    }


    public class resultat
    {
        public string  item { get; set; }
        public double? charge { get; set; }
        public double? cout { get; set; }
    }
}
