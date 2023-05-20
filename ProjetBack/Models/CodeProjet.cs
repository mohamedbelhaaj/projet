using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class CodeProjet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string Numero { get; set; }
        public string Nature { get; set; }
        public string Intitule { get; set; }
        public string ClientId { get; set; }
        public string? createur { get; set; }

        public DateTime? dateCreation { get; set; }
        public virtual  Client Client { get; set; }
        public virtual ICollection<ProjetEdp> ProjetEdps { get; set; }




    }
}
