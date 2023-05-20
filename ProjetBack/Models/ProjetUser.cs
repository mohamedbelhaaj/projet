using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class ProjetUser
    {
       
        public string UserId { get; set; }
        public string ProjetId { get; set; }

        public virtual User User { get; set; }
        public virtual Projet Projet { get; set; }
    }
}
