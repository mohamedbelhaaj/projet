using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Commentaire
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdCommentaire { get; set; }
        public string Description { get; set; }
        /*one to many relation*/
        public string Id { get; set; }
        public  DetailLivraison DetailLivraison { get; set;}

    }
}
