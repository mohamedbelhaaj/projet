using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class EFCoreCodeFirstSampleModelsPilotageDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhaseProjets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseProjets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profils",
                columns: table => new
                {
                    IdProfil = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profils", x => x.IdProfil);
                });

            migrationBuilder.CreateTable(
                name: "ProjetLivraisons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Delivery = table.Column<string>(nullable: true),
                    Planned = table.Column<bool>(nullable: false),
                    EbRc = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    StartDate = table.Column<string>(nullable: true),
                    InitialPlannedDate = table.Column<string>(nullable: true),
                    PlannedDate = table.Column<string>(nullable: true),
                    DeliveryDate = table.Column<string>(nullable: true),
                    DateCreation = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    DateDerniereModification = table.Column<string>(nullable: true),
                    Modificateur = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetLivraisons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IdStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IdStatus);
                });

            migrationBuilder.CreateTable(
                name: "StatusImputations",
                columns: table => new
                {
                    IdStatusImputations = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusImputations", x => x.IdStatusImputations);
                });

            migrationBuilder.CreateTable(
                name: "StatutTaches",
                columns: table => new
                {
                    IdStatutTache = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutTaches", x => x.IdStatutTache);
                });

            migrationBuilder.CreateTable(
                name: "TTM",
                columns: table => new
                {
                    IdTTM = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTM", x => x.IdTTM);
                });

            migrationBuilder.CreateTable(
                name: "TypeClients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Prenom = table.Column<string>(nullable: true),
                    Adresse = table.Column<string>(nullable: true),
                    AdresseEmail = table.Column<string>(nullable: true),
                    MotDePasse = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Valideur1 = table.Column<string>(nullable: true),
                    Valideur2 = table.Column<string>(nullable: true),
                    DateCreation = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    DateDerniereModification = table.Column<string>(nullable: true),
                    Modificateur = table.Column<string>(nullable: true),
                    IdProfil = table.Column<string>(nullable: true),
                    ProfilIdProfil = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Profils_ProfilIdProfil",
                        column: x => x.ProfilIdProfil,
                        principalTable: "Profils",
                        principalColumn: "IdProfil",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetailLivraisons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Delivery = table.Column<string>(nullable: true),
                    Planned = table.Column<bool>(nullable: false),
                    EBRC = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    StartDate = table.Column<string>(nullable: true),
                    InitialPlannedDate = table.Column<string>(nullable: true),
                    PlannedDate = table.Column<string>(nullable: true),
                    DeliveryDate = table.Column<string>(nullable: true),
                    DateCreation = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    DateDerniereModification = table.Column<string>(nullable: true),
                    Modificateur = table.Column<string>(nullable: true),
                    ProjetLivraisonId = table.Column<string>(nullable: true),
                    TTMId = table.Column<string>(nullable: true),
                    StatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailLivraisons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailLivraisons_ProjetLivraisons_ProjetLivraisonId",
                        column: x => x.ProjetLivraisonId,
                        principalTable: "ProjetLivraisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailLivraisons_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "IdStatus",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailLivraisons_TTM_TTMId",
                        column: x => x.TTMId,
                        principalTable: "TTM",
                        principalColumn: "IdTTM",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Adresse = table.Column<string>(nullable: true),
                    AdresseEmail = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    DateCreation = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    DateDerniereModification = table.Column<string>(nullable: true),
                    Modificateur = table.Column<string>(nullable: true),
                    TypeClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_TypeClients_TypeClientId",
                        column: x => x.TypeClientId,
                        principalTable: "TypeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Imputations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateDebut = table.Column<string>(nullable: true),
                    DateFin = table.Column<string>(nullable: true),
                    IdStatusImputations = table.Column<string>(nullable: true),
                    StatusImputationsIdStatusImputations = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    UserIdUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imputations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imputations_StatusImputations_StatusImputationsIdStatusImputations",
                        column: x => x.StatusImputationsIdStatusImputations,
                        principalTable: "StatusImputations",
                        principalColumn: "IdStatusImputations",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imputations_Users_UserIdUser",
                        column: x => x.UserIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commentaires",
                columns: table => new
                {
                    IdCommentaire = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true),
                    DetailLivraisonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commentaires", x => x.IdCommentaire);
                    table.ForeignKey(
                        name: "FK_Commentaires_DetailLivraisons_DetailLivraisonId",
                        column: x => x.DetailLivraisonId,
                        principalTable: "DetailLivraisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projets",
                columns: table => new
                {
                    IdProjet = table.Column<string>(nullable: false),
                    Nommenclature = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Publique = table.Column<string>(nullable: true),
                    DateCreation = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    DateDerniereModification = table.Column<string>(nullable: true),
                    Modificateur = table.Column<string>(nullable: true),
                    IdPhaseProjet = table.Column<string>(nullable: true),
                    PhaseProjetId = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projets", x => x.IdProjet);
                    table.ForeignKey(
                        name: "FK_Projets_DetailLivraisons_Id",
                        column: x => x.Id,
                        principalTable: "DetailLivraisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projets_PhaseProjets_PhaseProjetId",
                        column: x => x.PhaseProjetId,
                        principalTable: "PhaseProjets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjetLivraisonClients",
                columns: table => new
                {
                    ClientId = table.Column<string>(nullable: false),
                    ProjetLivraisonId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetLivraisonClients", x => new { x.ProjetLivraisonId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_ProjetLivraisonClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetLivraisonClients_ProjetLivraisons_ProjetLivraisonId",
                        column: x => x.ProjetLivraisonId,
                        principalTable: "ProjetLivraisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailImputations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Projet = table.Column<string>(nullable: true),
                    Client = table.Column<string>(nullable: true),
                    Phase = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Lundi = table.Column<string>(nullable: true),
                    Mardi = table.Column<string>(nullable: true),
                    Mercredi = table.Column<string>(nullable: true),
                    Jeudi = table.Column<string>(nullable: true),
                    Vendredi = table.Column<string>(nullable: true),
                    IdImputation = table.Column<string>(nullable: true),
                    ImputationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailImputations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailImputations_Imputations_ImputationId",
                        column: x => x.ImputationId,
                        principalTable: "Imputations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Taches",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Charge = table.Column<string>(nullable: true),
                    Createur = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    UserIdUser = table.Column<string>(nullable: true),
                    IdStatutTache = table.Column<string>(nullable: true),
                    StatutTacheIdStatutTache = table.Column<string>(nullable: true),
                    IdProjet = table.Column<string>(nullable: true),
                    ProjetIdProjet = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taches_Projets_ProjetIdProjet",
                        column: x => x.ProjetIdProjet,
                        principalTable: "Projets",
                        principalColumn: "IdProjet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Taches_StatutTaches_StatutTacheIdStatutTache",
                        column: x => x.StatutTacheIdStatutTache,
                        principalTable: "StatutTaches",
                        principalColumn: "IdStatutTache",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Taches_Users_UserIdUser",
                        column: x => x.UserIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Adresse", "AdresseEmail", "Createur", "DateCreation", "DateDerniereModification", "Fax", "Modificateur", "Nom", "Telephone", "TypeClientId" },
                values: new object[] { "1", "Trabelsi.Mohammed@gmail.com", null, null, null, null, "986524565658", null, "Trabelsi", "999-888-7777", null });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Adresse", "AdresseEmail", "Createur", "DateCreation", "DateDerniereModification", "Fax", "Modificateur", "Nom", "Telephone", "TypeClientId" },
                values: new object[] { " 2", "Trabelsi.Ali@gmail.com", null, null, null, null, "986524565658", null, "Trabelsi", "999-8528-7777", null });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TypeClientId",
                table: "Clients",
                column: "TypeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_DetailLivraisonId",
                table: "Commentaires",
                column: "DetailLivraisonId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailImputations_ImputationId",
                table: "DetailImputations",
                column: "ImputationId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_ProjetLivraisonId",
                table: "DetailLivraisons",
                column: "ProjetLivraisonId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_StatusId",
                table: "DetailLivraisons",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_TTMId",
                table: "DetailLivraisons",
                column: "TTMId");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationsIdStatusImputations",
                table: "Imputations",
                column: "StatusImputationsIdStatusImputations");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_UserIdUser",
                table: "Imputations",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisonClients_ClientId",
                table: "ProjetLivraisonClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projets_Id",
                table: "Projets",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projets_PhaseProjetId",
                table: "Projets",
                column: "PhaseProjetId");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_ProjetIdProjet",
                table: "Taches",
                column: "ProjetIdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_StatutTacheIdStatutTache",
                table: "Taches",
                column: "StatutTacheIdStatutTache");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_UserIdUser",
                table: "Taches",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilIdProfil",
                table: "Users",
                column: "ProfilIdProfil");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commentaires");

            migrationBuilder.DropTable(
                name: "DetailImputations");

            migrationBuilder.DropTable(
                name: "ProjetLivraisonClients");

            migrationBuilder.DropTable(
                name: "Taches");

            migrationBuilder.DropTable(
                name: "Imputations");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Projets");

            migrationBuilder.DropTable(
                name: "StatutTaches");

            migrationBuilder.DropTable(
                name: "StatusImputations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TypeClients");

            migrationBuilder.DropTable(
                name: "DetailLivraisons");

            migrationBuilder.DropTable(
                name: "PhaseProjets");

            migrationBuilder.DropTable(
                name: "Profils");

            migrationBuilder.DropTable(
                name: "ProjetLivraisons");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "TTM");
        }
    }
}
