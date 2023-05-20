using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetBack.Models
{
    public class ProjetEdp
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }

        public string CodeProjet { get; set; }

        public string Description { get; set; }
        public double? BudgetRallonge { get; set; }
        public string Nom { get; set; }
        public double BudgetInitial { get; set; }
        public string clientId { get; set; }
        public string Createur { get; set; }
public string? DateCreation { get; set; }
        public Client  client { get; set; }
        [ForeignKey("CodeProjets")]
        public string CodeProjetsId { get; set; }
        public DateTime? dateDebut { get; set; }
        public DateTime? dateFinInitial { get; set; }
        public DateTime? dateFinPrevue { get; set; }
        public string? ttm { get; set; }
        public DateTime? dateFin { get; set; }
        public double? budgetJunior { get; set; }
        public double? budgetSenior { get; set; }
        public double? budgetConfirme { get; set; }
        public double? budgetValidation { get; set; }
        public double? budgetDirection { get; set; }
        public double? budgetGP { get; set; }
        public string status { get; set; }

        public double? budgetJuniorRallonge { get; set; }
        public double? budgetSeniorRallonge { get; set; }
        public double? budgetConfirmeRallonge { get; set; }
        public double? budgetValidationRallonge { get; set; }
        public double? budgetDirectionRallonge { get; set; }
        public double? budgetGPRallonge { get; set; }


        public virtual CodeProjet CodeProjets { get; set; }

        public virtual ICollection<ProjetLivraison> ProjetLivraisons { get; set; }
        //public string CommercantId { get; set; }
        //public Commercant Commercant { get; set; }
        [ForeignKey("Commercial")]
        public string CommercialId { get; set; }

        public User Commercial { get; set; }
    }
}
