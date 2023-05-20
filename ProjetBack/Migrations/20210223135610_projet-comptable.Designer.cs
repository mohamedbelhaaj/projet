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
    [Migration("20210223135610_projet-comptable")]
    partial class projetcomptable
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

                    b.Property<bool>("lockoutEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Adresse = "Trabelsi.Mohammed@gmail.com",
                            Fax = "986524565658",
                            Nom = "Trabelsi",
                            Telephone = "999-888-7777",
                            lockoutEnabled = false
                        },
                        new
                        {
                            Id = " 2",
                            Adresse = "Trabelsi.Ali@gmail.com",
                            Fax = "986524565658",
                            Nom = "Trabelsi",
                            Telephone = "999-8528-7777",
                            lockoutEnabled = false
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

                    b.Property<string>("DateCreation")
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

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBRC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InitialPlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Planned")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProjetLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TTMId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProjetId");

                    b.HasIndex("ProjetLivraisonId");

                    b.ToTable("DetailLivraisons");
                });

            modelBuilder.Entity("DAL.Models.Imputation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<DateTime>("DateDebut")
                        .HasColumnType("datetime2");

                    b.Property<string>("DateDerniereModification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateFin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nommenclature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhaseProjetId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publique")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("lockoutEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

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

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EBRC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InitialPlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Modificateur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Planned")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PlannedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TTMId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("lockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("projetEdpId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("projetEdpId");

                    b.ToTable("ProjetLivraisons");
                });

            modelBuilder.Entity("DAL.Models.StatutTache", b =>
                {
                    b.Property<string>("IdStatutTache")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdStatutTache");

                    b.ToTable("StatutTaches");
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

                    b.Property<bool>("confirmed")
                        .HasColumnType("bit");

                    b.Property<string>("expertise")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastConnexion")
                        .HasColumnType("datetime2");

                    b.Property<bool>("lockoutEnabled")
                        .HasColumnType("bit");

                    b.HasKey("IdUser");

                    b.HasIndex("ProfilId");

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

            modelBuilder.Entity("ProjetBack.Models.ClientProjet", b =>
                {
                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjetId", "ClientId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientProjet");
                });

            modelBuilder.Entity("ProjetBack.Models.CodeProjet", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Intitule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("dateCreation")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("ClientId");

                    b.ToTable("CodeProjet");
                });

            modelBuilder.Entity("ProjetBack.Models.Commercant", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Commercants");
                });

            modelBuilder.Entity("ProjetBack.Models.Equipe", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManagerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Equips");
                });

            modelBuilder.Entity("ProjetBack.Models.EquipeUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EquipeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "EquipeId");

                    b.HasIndex("EquipeId");

                    b.ToTable("EquipeUser");
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetEdp", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BudgetInitial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BudgetRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeProjet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeProjetsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CommercantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Createur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetConfirmeRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetConfirmé")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetDirection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetDirectionRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetGP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetGPRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetJunior")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetJuniorRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetSenior")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetSeniorRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetValidation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("budgetValidationRallonge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("clientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("dateDebut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dateFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dateFinInitial")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("CodeProjetsId");

                    b.HasIndex("CommercantId");

                    b.HasIndex("clientId");

                    b.ToTable("ProjetEdps");
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetEquipe", b =>
                {
                    b.Property<string>("ProjetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EquipeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjetId", "EquipeId");

                    b.HasIndex("EquipeId");

                    b.ToTable("ProjetEquipes");
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

                    b.Property<string>("DateCreation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatutTacheId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("chargeConsomme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("detailLivraisonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("lienAzure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("premierImputation")
                        .HasColumnType("datetime2");

                    b.Property<bool>("publique")
                        .HasColumnType("bit");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StatutTacheId");

                    b.HasIndex("UserId");

                    b.HasIndex("detailLivraisonId");

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
                });

            modelBuilder.Entity("DAL.Models.Imputation", b =>
                {
                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("Imputations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Models.ProjetLivraison", b =>
                {
                    b.HasOne("DAL.Models.Client", "Client")
                        .WithMany("ProjetLivraisons")
                        .HasForeignKey("ClientId");

                    b.HasOne("ProjetBack.Models.ProjetEdp", "projetEdp")
                        .WithMany("ProjetLivraisons")
                        .HasForeignKey("projetEdpId");
                });

            modelBuilder.Entity("DAL.Models.User", b =>
                {
                    b.HasOne("DAL.Models.Profil", "Profil")
                        .WithMany("Users")
                        .HasForeignKey("ProfilId");
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

            modelBuilder.Entity("ProjetBack.Models.ClientProjet", b =>
                {
                    b.HasOne("DAL.Models.Client", "Client")
                        .WithMany("ClientProjet")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany("ClientProjets")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetBack.Models.CodeProjet", b =>
                {
                    b.HasOne("DAL.Models.Client", "Client")
                        .WithMany("CodeProjets")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("ProjetBack.Models.Equipe", b =>
                {
                    b.HasOne("DAL.Models.User", "Manager")
                        .WithMany("equips")
                        .HasForeignKey("ManagerId");
                });

            modelBuilder.Entity("ProjetBack.Models.EquipeUser", b =>
                {
                    b.HasOne("ProjetBack.Models.Equipe", "Equipe")
                        .WithMany("equipeUsers")
                        .HasForeignKey("EquipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("equipeUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetEdp", b =>
                {
                    b.HasOne("ProjetBack.Models.CodeProjet", "CodeProjets")
                        .WithMany("ProjetEdps")
                        .HasForeignKey("CodeProjetsId");

                    b.HasOne("ProjetBack.Models.Commercant", "Commercant")
                        .WithMany("ProjetEdp")
                        .HasForeignKey("CommercantId");

                    b.HasOne("DAL.Models.Client", "client")
                        .WithMany("ProjetEdps")
                        .HasForeignKey("clientId");
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetEquipe", b =>
                {
                    b.HasOne("ProjetBack.Models.Equipe", "Equipe")
                        .WithMany("projetsEquipe")
                        .HasForeignKey("EquipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Projet", "projet")
                        .WithMany("projetsEquipe")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetBack.Models.ProjetUser", b =>
                {
                    b.HasOne("DAL.Models.Projet", "Projet")
                        .WithMany()
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
                    b.HasOne("DAL.Models.StatutTache", "StatutTache")
                        .WithMany("Taches")
                        .HasForeignKey("StatutTacheId");

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("Taches")
                        .HasForeignKey("UserId");

                    b.HasOne("DAL.Models.DetailLivraison", "detailLivraison")
                        .WithMany("taches")
                        .HasForeignKey("detailLivraisonId");
                });
#pragma warning restore 612, 618
        }
    }
}
