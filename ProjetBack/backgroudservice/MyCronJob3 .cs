using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetBack.backgroudservice
{
    public class MyCronJob3 : CronJobService
    {
        private SqlConnection con;
        private readonly IConfiguration _config;

        private readonly ILogger<MyCronJob3> _logger;
        public MyCronJob3(IScheduleConfig<MyCronJob3> config, ILogger<MyCronJob3> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
      

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override  Task DoWork(CancellationToken cancellationToken)
        {



           _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 3 is working.");
            return  Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 is stopping.");
            return base.StopAsync(cancellationToken);
        }


        private void SendEmail(string email, string messageBody, string subject)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 25);

            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("mss.systeminfo@gmail.com", "mssolution");
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            mailMessage.From = new MailAddress("mss.systeminfo@gmail.com");
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = messageBody;
            client.Send(mailMessage);

        }

        //public Task SendAsync(string email, string messageBody, string subject)
        //{
        //    var _sender = "mss.systeminfo@gmail.com";
        //    var _user = _appSettings.EmailUser;
        //    var _password = "mssolution";
        //    var _smtpClient = _appSettings.Host;
        //    int _port = _appSettings.EmailPort;

        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient(_smtpClient);
        //    mail.From = new MailAddress(_sender);

        //    mail.To.Add(email);

        //    mail.Subject = subject;
        //    mail.Body = messageBody;

        //    SmtpServer.Port = _port;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential(_user, _password);
        //    SmtpServer.EnableSsl = true;
        //    try
        //    {
        //        SmtpServer.Send(mail);
        //    }
        //    catch { }
        //    return Task.FromResult(0);
        //}









        public void ChangeTTMProjetLivraisonAsync()
        {

            List<ProjetLivraison> projetLivraisons=new List<ProjetLivraison>();
            string test = "test";
            string queryString =
        "SELECT * from ProjetLivraisons where TTMId IS NULL and (StatusId = 'Running' OR StatusId = 'Delivered')";
            using (SqlConnection connection = new SqlConnection(
                       "Server=TEC-KHAOULAW;Database=PilotageDB3;Trusted_Connection=True;"))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    
                        if(DateTime.Parse(reader[7].ToString()).Date < DateTime.Now.Date && reader[14].ToString() == "Running"|| (DateTime.Parse(reader[9].ToString()) > DateTime.Parse(reader[7].ToString()) && reader[14].ToString() == "Delivered"))
                        {
                            var projet = new ProjetLivraison();
                            projet.Id = reader[0].ToString();
                            projet.TTMId = "late";
                            projetLivraisons.Add(projet);

                        }
                        else if (DateTime.Parse(reader[9].ToString()).Date == DateTime.Parse(reader[7].ToString()).Date && reader[14].ToString() == "Delivered")
                         {
                            var projet = new ProjetLivraison();
                            projet.Id = reader[0].ToString();
                            projet.TTMId = "ONTIME";
                            projetLivraisons.Add(projet);

                        }
                    }
                }
            }


            using (SqlConnection connection = new SqlConnection(
                      "Server=TEC-KHAOULAW;Database=PilotageDB3;Trusted_Connection=True;"))
            {
                foreach (var item in projetLivraisons)
                {
                    string queryStringUpdate =
"UPDATE ProjetLivraisons  SET TTMId ='"+ item.TTMId+ "' WHERE  Id='" + item.Id+"'";
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} crone success:  " +queryStringUpdate);
                    SqlCommand commandupdate = new SqlCommand(
          queryStringUpdate, connection);
                    connection.Open();
                    commandupdate.ExecuteNonQuery();
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} crone result update:" + commandupdate.ExecuteNonQuery());


                }
            }
         

        }


        public void ChangeTTMDetailProjetLivraisonAsync()
        {

            List<ProjetLivraison> projetLivraisons = new List<ProjetLivraison>();
           
            string queryString =
        "SELECT * from DetailLivraisons where TTMId IS NULL and (StatusId = 'Running' OR StatusId = 'Delivered')";
            using (SqlConnection connection = new SqlConnection(
                       "Server=TEC-KHAOULAW;Database=PilotageDB3;Trusted_Connection=True;"))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (DateTime.Parse(reader[7].ToString()).Date < DateTime.Now.Date && reader[14].ToString() == "Running" || (DateTime.Parse(reader[9].ToString()) > DateTime.Parse(reader[7].ToString()) && reader[14].ToString() == "Delivered"))
                        {
                            var projet = new ProjetLivraison();
                            projet.Id = reader[0].ToString();
                            projet.TTMId = "late";
                            projetLivraisons.Add(projet);

                        }
                        else if (DateTime.Parse(reader[9].ToString()).Date == DateTime.Parse(reader[7].ToString()).Date && reader[14].ToString() == "Delivered")
                        {
                            var projet = new ProjetLivraison();
                            projet.Id = reader[0].ToString();
                            projet.TTMId = "ONTIME";
                            projetLivraisons.Add(projet);

                        }
                    }
                }
            }


            using (SqlConnection connection = new SqlConnection(
                      "Server=TEC-KHAOULAW;Database=PilotageDB3;Trusted_Connection=True;"))
            {
                foreach (var item in projetLivraisons)
                {
                    string queryStringUpdate =
"UPDATE DetailLivraisons  SET TTMId ='" + item.TTMId + "' WHERE  Id='" + item.Id + "'";
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} crone success:  " + queryStringUpdate);
                    SqlCommand commandupdate = new SqlCommand(
          queryStringUpdate, connection);
                    connection.Open();
                    commandupdate.ExecuteNonQuery();
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} crone result update:" + commandupdate.ExecuteNonQuery());


                }
            }

        }
    }
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }



}
