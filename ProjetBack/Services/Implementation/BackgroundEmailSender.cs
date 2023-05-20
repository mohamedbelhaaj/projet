
using DAL.Models;
using Gateway.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetBack.Services.Interface;
using ProjetBack.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetBack.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly PilotageDBContext _context;
        private readonly IConfiguration config;

        // private readonly PilotageDBContext _context;

        public BackgroundEmailSender(PilotageDBContext _context, IConfiguration config)
        {
            this._context =_context;
            this.config = config;
        }

        public async Task<List<string>> GetManagerByUser(string id)
        {

            var dbresult = await _context.EquipeUser.
            Include(e => e.User).Where(e
             => e.UserId == id).Select(e => e.EquipeId).ToListAsync();
            var result2 = await _context.Equips
                .Include(e => e.Manager)
                .Where(e => dbresult.
                Contains(e.Id))
                .Select(e => e.Manager.AdresseEmail)
                .Distinct().ToListAsync();
            return result2;

        }



        public async Task DoWork()
        {
            DateTime startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            DateTime endDate = startDate.AddDays(4);            
            List<string> resultat = new List<string>();           
            HashSet<string> MailsManagers = new HashSet<string>();
            if (DateTime.Now.Date == endDate)
            {
                var result = await _context.Imputations.Where(x => x.StatusImputation != 1 && x.DateFin.Date == endDate && x.DateDebut.Date == startDate).Include(x => x.User).Select(x => x.UserId).ToListAsync();

                var result1 = await _context.Users.Include(e=>e.equips).Where(x => !result.Contains(x.IdUser) && (x.Type == UserType.User || x.Type == UserType.Manager)

                && (x.FirstImputation == null || x.FirstImputation <= endDate)

                              && (x.LastImputation >= endDate || x.LastImputation == null)
                ).ToListAsync();
                if (result1.Any())
                {

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(config.GetValue<string>("AppSettings:EmailSender"), config.GetValue<string>("AppSettings:Password"));
                    MailMessage mailMessage = new MailMessage();
                    result1.ForEach(e =>
                   {
                       mailMessage.To.Add(e.AdresseEmail);
                       (GetManagerByUser(e.IdUser).Result).ForEach(a =>
                       MailsManagers.Add(a));
                   });
                    foreach (var Email in MailsManagers)
                    {
                        mailMessage.CC.Add(Email);
                    }
                    mailMessage.CC.Distinct();
                    mailMessage.From = new MailAddress(config.GetValue<string>("AppSettings:EmailSender"));
                    mailMessage.Subject = "Rappel Imputation";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = "Bonjour,\n \n Veuillez vérifier vos imputations.\n \n Bonne journée.";
                    client.Send(mailMessage);
                }
        
            }




        }
    }
}
