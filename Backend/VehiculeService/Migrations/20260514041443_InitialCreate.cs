using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiculeService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Couleur = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Matricule = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VitesseLimite = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Capacite = table.Column<int>(type: "int", nullable: true),
                    NbrEssieux = table.Column<int>(type: "int", nullable: true),
                    NbrPlaces = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vehicules",
                columns: new[] { "Id", "Capacite", "Couleur", "Marque", "Matricule", "NbrEssieux", "Type", "VitesseLimite" },
                values: new object[] { 1, 12000, "Blanc", "Volvo", "TU-1234", 3, "Camion", 90 });

            migrationBuilder.InsertData(
                table: "Vehicules",
                columns: new[] { "Id", "Couleur", "Marque", "Matricule", "NbrPlaces", "Type", "VitesseLimite" },
                values: new object[] { 2, "Gris", "Peugeot", "TU-5678", 5, "Voiture", 120 });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicules_Matricule",
                table: "Vehicules",
                column: "Matricule",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicules");
        }
    }
}
