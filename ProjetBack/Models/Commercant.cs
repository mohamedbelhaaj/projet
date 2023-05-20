using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class Commercant
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string  id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string FullName => Prenom + " " + Nom;

        public virtual ICollection<ProjetEdp> ProjetEdp { get; set; }

        public string Createur { get; set; }
        public string? DateCreation { get; set; }


    }
}
