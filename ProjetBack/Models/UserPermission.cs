using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
  public class UserPermission
    { 
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]        
        public string UserId { get; set; }
        
        public string PermissionId { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }


    }


}
