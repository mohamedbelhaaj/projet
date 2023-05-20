using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class EquipeUser
    {
        public string UserId { get; set; }
        public string EquipeId { get; set; }
        public User User { get; set; }
        public Equipe Equipe { get; set; }
        public string role { get; set; }
    }
}
