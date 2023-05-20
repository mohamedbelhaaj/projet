using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
  public  class Profil
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdProfil { get; set; }
  

        public ICollection<User> Users { get; set; }

    }
}
