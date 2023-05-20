using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.PushServices
{
    public interface INotificationHubService
    {
        Task GetNotifications(params Notification[] notifications);
        Task GetNotifications(object list);
    }
}
