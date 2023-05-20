using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class Profile
    {
        public string profileId { get; set; }
        public string annee { get; set; }
        public string budget { get; set; }
        public virtual ICollection <profileUser> profileUser { get; set; }



    }
}
