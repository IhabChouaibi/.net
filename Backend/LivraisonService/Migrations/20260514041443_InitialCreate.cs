using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LivraisonService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdressesLivraison",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresse = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodePostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ColisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressesLivraison", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatutsLivraison",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutsLivraison", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdressesLivraison",
                columns: new[] { "Id", "Adresse", "CodePostal", "ColisId", "Ville" },
                values: new object[,]
                {
                    { 1, "10 Rue de Marseille", "1000", 1, "Tunis" },
                    { 2, "25 Avenue Habib Bourguiba", "3000", 2, "Sfax" },
                    { 3, "8 Rue des Oliviers", "4000", 3, "Sousse" }
                });

            migrationBuilder.InsertData(
                table: "StatutsLivraison",
                columns: new[] { "Id", "Libelle" },
                values: new object[,]
                {
                    { 1, "En attente" },
                    { 2, "En cours" },
                    { 3, "Livre" },
                    { 4, "Annule" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdressesLivraison");

            migrationBuilder.DropTable(
                name: "StatutsLivraison");
        }
    }
}
