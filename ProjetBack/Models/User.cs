using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class User
    {

        public User()
        {
            Taches = new HashSet<Tache>();
            UserPermissions = new HashSet<UserPermission>();
        }

        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdUser { get; set; }
        public Boolean confirmed { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string FullName => Prenom  + " " + Nom;
        

        public string Adresse { get; set; }
        public DateTime? LastImputation { get; set; }
        public DateTime? FirstImputation { get; set; }
       
        public string AdresseEmail { get; set; }
        public string expertise { get; set; }
        public string MotDePasse { get; set; }

        public UserType Type { get; set; }
        //public  string EquipeId { get; set; }
        //public  Equipe Equipe{ get; set; }

        public virtual ICollection<Equipe> equips { get; set; }

        public virtual ICollection<EquipeUser> equipeUsers { get; set; }
        public virtual ICollection<ProjetEdp> ProjetsComercieaux { get; set; }
        public virtual ICollection<profileUser> profileUser { get; set; }

        public string Telephone { get; set; }
 
        public Boolean lockoutEnabled { get; set; }
        public DateTime? lastConnexion { get; set; }
        public string Createur { get; set; }
        public string? DateCreation { get; set; }
        public string DateDerniereModification { get; set; }
       
        public string Modificateur { get; set; }
        
        public ICollection<Imputation> Imputations { get; set; }
   
        public string ProfilId { get; set; }
        
        public ICollection<Tache> Taches { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }

        public virtual ICollection<ProjetUser> ProjetUsers { get; set; }
        public DateTime? ResetPassword { get; set; }

    }

    public enum UserType
    {
        Admin = 'a',
        Manager = 'm',
        User = 'u',
        Commercial ='c',

        SuperCommercial = 's',

    }

    public static class RoleType
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string SuperCommercial = "SuperCommercial";
        public const string User = "User";
        public const string Commercial = "Commercial";

        public static string GetRole(UserType type)
        {
            string role = string.Empty;
            switch (type)
            {
                case UserType.Admin:
                    role = Admin;
                    break;
                case UserType.SuperCommercial:
                    role = SuperCommercial;
                    break;
                case UserType.Manager:
                    role = Manager;
                    break;
                case UserType.User:
                    role = User;
                    break;
                case UserType.Commercial:
                    role = Commercial;
                    break;
                default: break;
            }
            return role;


        }

    }
}
