using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
   public class ProjetLivraison : ProjectBase
    {



        //public string ProjetId { get; set; }
        public string ProjetName { get; set; }
        public Boolean lockoutEnabled { get; set; }
        public string Nature  { get; set; }
        public string? scope  { get; set; }



        /*one-to-many*/
        public virtual ICollection<DetailLivraison> DetailLivraisons { get; set; }
        /*many-to-many Client et ProjetLivraison */
        // public virtual  ICollection<ProjetLivraisonClient> ProjetLivraisonClients { get; set; }


        /*one to many projetLivraison et client */
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public string? projetEdpId { get; set; }
        public ProjetEdp projetEdp { get; set; }

    
    }
}
