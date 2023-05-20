using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
  public  class Projet
    { 
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Nommenclature { get; set; }
        public string Description { get; set; }
        public string Publique { get; set; }
        public string Type { get; set; }

        public string? DateCreation { get; set; }
        public string Createur { get; set; }
        public string DateDerniereModification { get; set; }
        public string Modificateur { get; set; }

        public virtual ICollection<ProjetEquipe> projetsEquipe { get; set; }
        public virtual ICollection<DetailLivraison> DetailLivraisons { get; set; }
        public virtual ICollection<ClientProjet> ClientProjets { get; set; }

        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }


        /*one to many*/

        public string PhaseProjetId{ get; set; }
        public Boolean lockoutEnabled { get; set; }
        // public ICollection<Client> Clients { get; set; }

        /*one-to-many*/
        //  public virtual ICollection<DetailLivraison> DetailLivraisons { get; set; }


        /* one to many :tache et projet*/
        //   public ICollection<Tache> Taches { get; set; }

        //    public virtual ICollection<ProjetUser> ProjetUsers { get; set; }

        // public virtual ICollection<ProjetLivraison> ProjetsLivraison { get; set; }



    }


}
