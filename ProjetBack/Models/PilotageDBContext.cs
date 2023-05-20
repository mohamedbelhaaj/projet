using Microsoft.EntityFrameworkCore;
using ProjetBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PilotageDBContext : DbContext
    {
        public PilotageDBContext(DbContextOptions options)
: base(options)
        {
        }
        /* DbSet  représente les entités qui seront par la suite transformé en tables*/
        public DbSet<Client> Clients { get; set; }
     
        //modifier
    //    public DbSet<ProjetLivraisonClient> ProjetLivraisonClients { get; set; }

        public DbSet<ProjetUser> ProjetUsers { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }


        public DbSet<Projet> Projets { get; set; }

        public DbSet<Tache> Taches { get; set; }
        public DbSet<StatutTache> StatutTaches { get; set; }


        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<DetailImputation> DetailImputations { get; set; }
        public DbSet<DetailLivraison> DetailLivraisons { get; set; }
        public DbSet<ProjetEquipe> ProjetEquipes { get; set; }

        
        public DbSet<ClientProjet> ClientProjet { get; set; }

        public DbSet<Imputation> Imputations { get; set; }
        public DbSet<Commercant> Commercants  { get; set; }
        public DbSet<ProjetEdp> ProjetEdps { get; set; }
        public DbSet<EquipeUser> EquipeUser { get; set; }

        public DbSet<Profil> Profils { get; set; }
        public DbSet<ProjetLivraison> ProjetLivraisons { get; set; }
        public DbSet<profileUser> profileUser { get; set; }
        public DbSet<Profile> profile { get; set; }

        public DbSet<Equipe> Equips { get; set; }





        /*Creation des données dans la base de données,on ajoute la methode OnModelCreating*/
        /*relation many-to-many ,on ajoute code pour table de jonction dans la methode OnModelCreating*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  /*table de joction(many-to-many) ProjetLivraisonClient à l'aide de API Fluent*/
         //   modelBuilder.Entity<ProjetLivraisonClient>().HasKey(sc => new { sc.ProjetLivraisonId, sc.ClientId });
            modelBuilder.Entity<User>()
    .HasMany(c => c.equips)
    .WithOne(e => e.Manager);



            //        modelBuilder.Entity<Equipe>()
            //.HasMany(c => c.Users)
            //.WithOne(e => e.Equipe);
            //      modelBuilder.Entity<ProjetEdp>()
            //.HasIndex(u => u.CodeProjet)
            //.IsUnique();
            modelBuilder.Entity<profileUser>()
  .HasKey(x => new { x.userId, x.profileId });

            modelBuilder.Entity<ProjetEquipe>()
       .HasKey(x => new { x.ProjetId, x.EquipeId });
      
            modelBuilder.Entity<EquipeUser>()
      .HasKey(x => new { x.UserId, x.EquipeId });



            modelBuilder.Entity<profileUser>()
         .HasOne(x => x.Profile)
         .WithMany(y => y.profileUser)
         .HasForeignKey(y => y.profileId);



            modelBuilder.Entity<profileUser>()
.HasOne(x => x.user)
.WithMany(y => y.profileUser)
.HasForeignKey(y => y.userId);

            modelBuilder.Entity<EquipeUser>()
                .HasOne(x => x.Equipe)
                .WithMany(y => y.equipeUsers)
                .HasForeignKey(y => y.EquipeId);
            modelBuilder.Entity<EquipeUser>()
         .HasOne(x => x.User)
         .WithMany(y => y.equipeUsers)
         .HasForeignKey(y => y.UserId);

            modelBuilder.Entity<ProjetEquipe>()
                .HasOne(x => x.Equipe)
                .WithMany(y => y.projetsEquipe)
                .HasForeignKey(y => y.EquipeId);

            modelBuilder.Entity<ProjetEquipe>()
                .HasOne(x => x.projet)
                .WithMany(y => y.projetsEquipe)
                .HasForeignKey(y => y.ProjetId);




            modelBuilder.Entity<DetailLivraison>()
                .HasOne(x => x.Projet)
                .WithMany(y => y.DetailLivraisons)
                .HasForeignKey(y => y.ProjetId);





            modelBuilder.Entity<ClientProjet>()
       .HasKey(x => new { x.ProjetId, x.ClientId });

            modelBuilder.Entity<ClientProjet>()
                .HasOne(x => x.Projet)
                .WithMany(y => y.ClientProjets)
                .HasForeignKey(y => y.ProjetId);

            modelBuilder.Entity<ClientProjet>()
                .HasOne(x => x.Client)
                .WithMany(y => y.ClientProjet)
                .HasForeignKey(y => y.ClientId);


         //   modelBuilder.Entity<ProjetLivraisonClient>().HasKey(sc => new { sc.ProjetLivraisonId, sc.ClientId });

            modelBuilder.Entity<ProjetUser>().HasKey(sc => new { sc.ProjetId, sc.UserId });

            modelBuilder.Entity<DetailLivraison>()
    .HasKey(o => new { o.Id });

            /*crreation des données*/
            _ = modelBuilder.Entity<Client>().HasData(new Client
            {
                Id = "1",
                Nom = "Trabelsi",
                Adresse = "Trabelsi.Mohammed@gmail.com",
                Telephone = "999-888-7777",
                Fax = "986524565658"

            }, new Client
            {
                Id = " 2"
                ,
                Nom = "Trabelsi",

                Adresse = "Trabelsi.Ali@gmail.com",
                Telephone = "999-8528-7777",
                Fax = "986524565658"

            });
        }





        /*Creation des données dans la base de données,on ajoute la methode OnModelCreating*/
        /*relation many-to-many ,on ajoute code pour table de jonction dans la methode OnModelCreating*/
        public DbSet<ProjetBack.Models.CodeProjet> CodeProjet { get; set; }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var stringKeyEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added);
        //    foreach (var entity in stringKeyEntries)
        //    {
        //        var primaryKeyName = entity.Metadata.FindPrimaryKey().Properties.FirstOrDefault(p => p.PropertyInfo.PropertyType == typeof(string))?.Name;
        //        if (!string.IsNullOrWhiteSpace(primaryKeyName))
        //        {
        //            entity.Property(primaryKeyName).CurrentValue = Guid.NewGuid().ToString();
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}

