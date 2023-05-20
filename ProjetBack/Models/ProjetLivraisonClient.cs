using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
   public class ProjetLivraisonClient
    {
    
   
        public string ClientId { get; set; }      
        public string ProjetLivraisonId { get; set; }

      
        public virtual Client Client { get; set; }
        public virtual ProjetLivraison ProjetLivraison { get; set; }
    }
}
