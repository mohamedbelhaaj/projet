using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class ProjetEquipe
    {
        public string  ProjetId{ get; set; }
        public string EquipeId { get; set; }
        public Projet projet { get; set; }
        public Equipe Equipe { get; set; }




    }
}
