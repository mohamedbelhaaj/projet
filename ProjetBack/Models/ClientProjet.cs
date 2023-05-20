using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class ClientProjet
    {
        public string ProjetId { get; set; }
        public Projet Projet { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
    }
}
