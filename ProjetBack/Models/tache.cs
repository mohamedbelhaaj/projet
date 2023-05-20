using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class Tache
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Description { get; set; }
        public string Charge { get; set; }
        public string chargeConsomme { get; set; }
        public string lienAzure { get; set; }

        public string Createur { get; set; }
        public DateTime? premierImputation { get; set; }
        public string? DateCreation { get; set; }

        public string status { get; set; }

        /*one to many tache et user*/

        public string? UserId { get; set; }
        public User? User { get; set; }
        public Boolean publique { get; set; }


        /*one to many tache et statutTache*/

        public string StatutTacheId { get; set; }
        public StatutTache StatutTache { get; set; }

        /*one to many tache et projet */

        public string detailLivraisonId { get; set; }
        public DetailLivraison detailLivraison { get; set; }
        public virtual ICollection<DetailImputation> DetailImputations { get; set; }

    }
}
