using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColisService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DateLivraison = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Poids = table.Column<double>(type: "float", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LivreurId = table.Column<int>(type: "int", nullable: false),
                    StatutLivraisonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colis", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Colis",
                columns: new[] { "Id", "ClientId", "DateLivraison", "Libelle", "LivreurId", "Montant", "Poids", "StatutLivraisonId", "Volume" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ordinateur portable", 1, 2200m, 2.5, 3, 0.01 },
                    { 2, 2, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Documents administratifs", 2, 35m, 0.20000000000000001, 1, 0.002 },
                    { 3, 3, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pieces auto", 3, 780m, 12.300000000000001, 2, 0.14999999999999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colis");
        }
    }
}
