using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Imputation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime? mondayDate { get; set; }
        public DateTime? TuesdayDate { get; set; }
        public DateTime? WednesdayDate { get; set; }
        public DateTime? FridayDate { get; set; }
        public DateTime? ThursdayDate { get; set; }

        public ICollection<DetailImputation> DetailImputations { get; set; }
        /*one-to-many*/
        public int StatusImputation { get; set; }
public string? DateCreation { get; set; }
        /*one-to-many */
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime? DateAvalide { get; set; }
    }
}
