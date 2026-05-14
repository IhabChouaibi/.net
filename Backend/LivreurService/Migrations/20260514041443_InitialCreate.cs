using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LivreurService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livreurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RaisonSocial = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CodePostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livreurs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Livreurs",
                columns: new[] { "Id", "CIN", "CodePostal", "RaisonSocial", "Ville" },
                values: new object[,]
                {
                    { 1, "12345678", "1000", "Express Nord", "Tunis" },
                    { 2, "87654321", "3000", "Rapid Sud", "Sfax" },
                    { 3, "11223344", "4000", "Livraison Centre", "Sousse" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livreurs_CIN",
                table: "Livreurs",
                column: "CIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livreurs");
        }
    }
}
