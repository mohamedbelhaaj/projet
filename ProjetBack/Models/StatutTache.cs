using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class StatutTache
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdStatutTache { get; set; }
        /*one_to_many*/
        public ICollection<Tache> Taches { get; set; }

    }
}
