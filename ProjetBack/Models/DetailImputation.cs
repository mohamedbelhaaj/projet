
using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
  public  class DetailImputation

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string IdClient { get; set; }
        public string IdProjet { get; set; }
        public string TacheId { get; set; }
        public Tache tache { get; set; }
        public string Charge { get; set; }
        public string IdStatutTache { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        /*one to many*/
        public string ImputationId { get; set; }
       // public int UserId { get; set; }
        public Imputation Imputation { get; set; }

public string? DateCreation { get; set; }

    }
}
