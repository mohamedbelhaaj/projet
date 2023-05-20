using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class profileUser
    {
        public string profileId { get; set; }
        public string userId { get; set; }
        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }

        public virtual User  user { get; set; }
        public virtual Profile  Profile { get; set; }


    }
}
