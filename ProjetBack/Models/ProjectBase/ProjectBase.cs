using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ProjectBase
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Description { get; set; }
        public string Delivery { get; set; }
        public Boolean Planned { get; set; }

        public string EBRC { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime InitialPlannedDate { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
public string? DateCreation { get; set; }
        public string Createur { get; set; }
        public string DateDerniereModification { get; set; }
        public string Modificateur { get; set; }
        /*les relations entre tables */
       
        public ICollection<Commentaire> Commentaires { get; set; }
        /* one to many*/
        public string TTMId { get; set; }
        /* one to many*/
        public string StatusId { get; set; }

    }
}
