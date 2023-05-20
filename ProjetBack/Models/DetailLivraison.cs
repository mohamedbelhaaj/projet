using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
  public  class DetailLivraison : ProjectBase
    {
        

        /*les relations entre tables */
        public string ProjetLivraisonId { get; set; }
        public ProjetLivraison ProjetLivraison { get; set; }
       
        /*one-to-many : Project and DetailLivraison*/
        public string ProjetId { get; set; }
        public Projet Projet { get; set; }
        //public string ClientId { get; set; }
        //public Client Client { get; set; }
        public virtual ICollection<Tache> taches { get; set; }



    }
}
