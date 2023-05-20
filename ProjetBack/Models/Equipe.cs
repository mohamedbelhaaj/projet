using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class Equipe
    {

        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Nom { get; set; }
       // public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ProjetEquipe> projetsEquipe { get; set; }

        public string ManagerId { get; set; }
        public virtual User Manager { get; set; }
        public virtual ICollection<EquipeUser> equipeUsers { get; set; }
        public string Createur { get; set; }
public string? DateCreation { get; set; }
    }
}
