using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetBack.PushServices
{
    [Authorize]    
    public class NotificationHub: Hub<INotificationHubService>
    {
        private readonly PilotageDBContext _dbContext;

        public NotificationHub(PilotageDBContext context)
        {

            this._dbContext = context;
        }
        public override async Task OnConnectedAsync()
        {
            // I get the role of user and then
            var userRole = this.Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var userId = this.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
         //   var userr = this.Context.User.Claims..FindFirstValue(ClaimTypes.NameIdentifier);
            // put user to group, so i can redirect notiification to specific role as manager or admin or use
            await  Groups.AddToGroupAsync(Context.ConnectionId,userRole);
            // get the old notification if(role is manager I get timesheets that sent by user), if( role is engineer i need to get
            // timesheets accepted by manager... i need to implement this part and condition for bello code
            if (userRole == RoleType.Admin)
            {
                //var notifications = await _dbContext.Imputations.Where(x => true).Select(x => new Notification { Title = x.Id, Content = "", ActionLink = "" }).ToArrayAsync();
                //await Clients.Client(Context.ConnectionId).GetNotifications(notifications);


            

                var notifications = await  (from Imputations in _dbContext.Imputations
                                    join user in _dbContext.Users on Imputations.UserId equals user.IdUser
                               
                                    
                                    where Imputations.StatusImputation== 2 
                                            select new
                                    {

                                        FullName = user.Nom + " " + user.Prenom,
                                        DateDebut = Imputations.DateDebut.Date.ToString(),
                                        DateFin = Imputations.DateFin.Date.ToString()
                                            }).Select(x => new Notification
                                    {
                                        Title = x.FullName + "a ajouté une Imputation ",
                                        Content = x.DateDebut + "/" + x.DateFin,
                                        ActionLink = "/liste_imputation",
                                        startDate = x.DateDebut,
                                        endDate = x.DateFin,
                                        
                                    }).ToArrayAsync();
                await Clients.Client(Context.ConnectionId).GetNotifications(notifications);
            }
            else if (userRole == RoleType.Manager )
                    {
                var notifications = await (from Imputations in _dbContext.Imputations
                                           join user in _dbContext.Users on Imputations.UserId equals user.IdUser

                                           join equipeUser in _dbContext.EquipeUser on user.IdUser equals equipeUser.UserId
                                           where Imputations.StatusImputation == 2 && equipeUser.Equipe.ManagerId == userId
                                           select new
                                           {

                                               FullName = user.Nom + " " + user.Prenom,
                                               DateDebut = Imputations.DateDebut.Date.ToString(),
                                               DateFin = Imputations.DateFin.Date.ToString()
                                           }).Select(x => new Notification
                                           {
                                               Title = x.FullName + "a ajouté une Imputation ",
                                               Content = x.DateDebut + "/" + x.DateFin,
                                               ActionLink = "/liste_imputation",
                                               startDate = x.DateDebut,
                                               endDate = x.DateFin,

                                           }).Distinct().ToArrayAsync();

               await Clients.Client(Context.ConnectionId).GetNotifications(notifications);
            }


            await  base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, this.Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
