﻿// <auto-generated />
using System;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjetBack.Migrations
{
    [DbContext(typeof(PilotageDBContext))]
    [Migration("20201101091408_delete-typeclient")]
    partial class deletetypeclient
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Models.Client", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Adresse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdresseEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeClientId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Adresse = "Trabelsi.Mohammed@gmail.com",
                            Fax = "986524565658",
                            Nom = "Trabelsi",
                            Telephone = "999-888-7777"
                        },
                        new
                        {
                            Id = " 2",
                            Adresse = "Trabelsi.Ali@gmail.com",
                            Fax = "986524565658",
                            Nom = "Trabelsi",
                            Telephone = "999-8528-7777"
                        });
                });

            modelBuilder.Entity("DAL.Models.Commentaire", b =>
                {
                    b.Property<string>("IdCommentaire")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DetailLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjetLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCommentaire");

                    b.HasIndex("DetailLivraisonId");

                    b.HasIndex("ProjetLivraisonId");

                    b.ToTable("Commentaires");
                });

            modelBuilder.Entity("DAL.Models.DetailImputation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Charge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Friday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdClient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdProjet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdStatutTache")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImputationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Monday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TacheId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Thursday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tuesday")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Wednesday")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ImputationId");

                    b.HasIndex("TacheId");

                    b.ToTable("DetailImputations");
                });

            modelBuilder.Entity("DAL.Models.DetailLivraison", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Delivery")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBRC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitialPlannedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Planned")
                        .HasColumnType("bit");

                    b.Property<string>("PlannedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProjetLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StartDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TTMId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjetId");

                    b.HasIndex("ProjetLivraisonId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TTMId");

                    b.ToTable("DetailLivraisons");
                });

            modelBuilder.Entity("DAL.Models.Imputation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DateDebut")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateFin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusImputation")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Imputations");
                });

            modelBuilder.Entity("DAL.Models.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(350)")
                        .HasMaxLength(350);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DAL.Models.PhaseProjet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("PhaseProjets");
                });

            modelBuilder.Entity("DAL.Models.Profil", b =>
                {
                    b.Property<string>("IdProfil")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdProfil");

                    b.ToTable("Profils");
                });

            modelBuilder.Entity("DAL.Models.Projet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdClient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nommenclature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhaseProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Publique")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PhaseProjetId");

                    b.ToTable("Projets");
                });

            modelBuilder.Entity("DAL.Models.ProjetLivraison", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Delivery")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBRC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitialPlannedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Planned")
                        .HasColumnType("bit");

                    b.Property<string>("PlannedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StartDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TTMId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProjetId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TTMId");

                    b.ToTable("ProjetLivraisons");
                });

            modelBuilder.Entity("DAL.Models.ProjetLivraisonClient", b =>
                {
                    b.Property<string>("ProjetLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjetLivraisonId", "ClientId");

                    b.HasIndex("ClientId");

                    b.ToTable("ProjetLivraisonClients");
                });

            modelBuilder.Entity("DAL.Models.Status", b =>
                {
                    b.Property<string>("IdStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdStatus");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("DAL.Models.StatutTache", b =>
                {
                    b.Property<string>("IdStatutTache")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdStatutTache");

                    b.ToTable("StatutTaches");
                });

            modelBuilder.Entity("DAL.Models.TTM", b =>
                {
                    b.Property<string>("IdTTM")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdTTM");

                    b.ToTable("TTM");
                });

            modelBuilder.Entity("DAL.Models.User", b =>
                {
                    b.Property<string>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Adresse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdresseEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EquipeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotDePasse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ResetPassword")
                        .HasColumnType("datetime2");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Valideur1Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Valideur2Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("confirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("lastConnexion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("lockoutEnabled")
                        .HasColumnType("bit");

                    b.HasKey("IdUser");

                    b.HasIndex("EquipeId");

                    b.HasIndex("ProfilId");

                    b.HasIndex("Valideur1Id");

                    b.HasIndex("Valideur2Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.Models.UserPermission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PermissionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("ProjetBack.Models.Equipe", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ManagerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Equips");
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetUser", b =>
                {
                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjetId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjetUsers");
                });

            modelBuilder.Entity("ProjetBack.Models.Tache", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Charge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StatutTacheId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProjetId");

                    b.HasIndex("StatutTacheId");

                    b.HasIndex("UserId");

                    b.ToTable("Taches");
                });

            modelBuilder.Entity("DAL.Models.Commentaire", b =>
                {
                    b.HasOne("DAL.Models.DetailLivraison", "DetailLivraison")
                        .WithMany("Commentaires")
                        .HasForeignKey("DetailLivraisonId");

                    b.HasOne("DAL.Models.ProjetLivraison", null)
                        .WithMany("Commentaires")
                        .HasForeignKey("ProjetLivraisonId");
                });

            modelBuilder.Entity("DAL.Models.DetailImputation", b =>
                {
                    b.HasOne("DAL.Models.Imputation", "Imputation")
                        .WithMany("DetailImputations")
                        .HasForeignKey("ImputationId");

                    b.HasOne("ProjetBack.Models.Tache", "tache")
                        .WithMany("DetailImputations")
                        .HasForeignKey("TacheId");
                });

            modelBuilder.Entity("DAL.Models.DetailLivraison", b =>
                {
                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany("DetailLivraisons")
                        .HasForeignKey("ProjetId");

                    b.HasOne("DAL.Models.ProjetLivraison", "ProjetLivraison")
                        .WithMany("DetailLivraisons")
                        .HasForeignKey("ProjetLivraisonId");

                    b.HasOne("DAL.Models.Status", "Status")
                        .WithMany("DetailLivraisons")
                        .HasForeignKey("StatusId");

                    b.HasOne("DAL.Models.TTM", "TTM")
                        .WithMany("DetailLivraisons")
                        .HasForeignKey("TTMId");
                });

            modelBuilder.Entity("DAL.Models.Imputation", b =>
                {
                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("Imputations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Models.Projet", b =>
                {
                    b.HasOne("DAL.Models.PhaseProjet", "PhaseProjet")
                        .WithMany("Projets")
                        .HasForeignKey("PhaseProjetId");
                });

            modelBuilder.Entity("DAL.Models.ProjetLivraison", b =>
                {
                    b.HasOne("DAL.Models.Client", "Client")
                        .WithMany("ProjetLivraisons")
                        .HasForeignKey("ClientId");

                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany("ProjetsLivraison")
                        .HasForeignKey("ProjetId");

                    b.HasOne("DAL.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("DAL.Models.TTM", "TTM")
                        .WithMany()
                        .HasForeignKey("TTMId");
                });

            modelBuilder.Entity("DAL.Models.ProjetLivraisonClient", b =>
                {
                    b.HasOne("DAL.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.ProjetLivraison", "ProjetLivraison")
                        .WithMany()
                        .HasForeignKey("ProjetLivraisonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Models.User", b =>
                {
                    b.HasOne("ProjetBack.Models.Equipe", "Equipe")
                        .WithMany("Users")
                        .HasForeignKey("EquipeId");

                    b.HasOne("DAL.Models.Profil", "Profil")
                        .WithMany("Users")
                        .HasForeignKey("ProfilId");

                    b.HasOne("DAL.Models.User", "Valideur1")
                        .WithMany()
                        .HasForeignKey("Valideur1Id");

                    b.HasOne("DAL.Models.User", "Valideur2")
                        .WithMany()
                        .HasForeignKey("Valideur2Id");
                });

            modelBuilder.Entity("DAL.Models.UserPermission", b =>
                {
                    b.HasOne("DAL.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetBack.Models.Equipe", b =>
                {
                    b.HasOne("DAL.Models.User", "Manager")
                        .WithMany("equips")
                        .HasForeignKey("ManagerId");
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetUser", b =>
                {
                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany("ProjetUsers")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("ProjetUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetBack.Models.Tache", b =>
                {
                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany("Taches")
                        .HasForeignKey("ProjetId");

                    b.HasOne("DAL.Models.StatutTache", "StatutTache")
                        .WithMany("Taches")
                        .HasForeignKey("StatutTacheId");

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("Taches")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
