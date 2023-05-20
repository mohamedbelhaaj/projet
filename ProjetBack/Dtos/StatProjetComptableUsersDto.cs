using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Dtos
{
    public class StatProjetComptableUsersDto
    {
        public string DateDebut { get; set; }
        public String  DateFin { get; set; }
        public List<String> ListUsers { get; set; }
        public int size { get; set; }
        public int page { get; set; }
        public List<String> ListClient { get; set; }



    }
}
