using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Client
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Nom { get; set; }

        public string Adresse { get; set; }
        public string AdresseEmail { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }

        // better to be date time
        public string DateCreation {get;set;}
        public string Createur { get; set; }
        public string DateDerniereModification { get; set; }
        public string Modificateur { get; set; }
        /*one-to-many Client et TypeClient*/
       
       public string TypeClientId { get; set; }
        public Boolean lockoutEnabled { get; set; }
        public virtual ICollection<ClientProjet> ClientProjet { get; set; }

        public virtual ICollection<ProjetEdp> ProjetEdps { get; set; }
        public virtual ICollection<CodeProjet> CodeProjets { get; set; }


        /*many-to-many Client and ProjetLivraison */
        //public virtual ICollection<ProjetLivraisonClient> ProjetLivraisonClients { get; set; }
        //modifier
        /*one to many projetLivraison et client*/
        public ICollection<ProjetLivraison> ProjetLivraisons { get; set; }


    }
}
