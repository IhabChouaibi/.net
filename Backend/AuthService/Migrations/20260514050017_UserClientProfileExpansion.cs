using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class UserClientProfileExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Comptes",
                type: "nvarchar(180)",
                maxLength: 180,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CIN",
                table: "Comptes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodePostal",
                table: "Comptes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Comptes",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Comptes",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "Comptes",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Comptes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "Comptes",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Adresse", "CIN", "CodePostal", "Email", "Nom", "Prenom", "Telephone", "Ville" },
                values: new object[] { "1 Avenue Admin", "ADMIN001", "1000", "admin@livraison.local", "Admin", "System", "70000001", "Tunis" });

            migrationBuilder.UpdateData(
                table: "Comptes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Adresse", "CIN", "CodePostal", "Email", "Nom", "Prenom", "Telephone", "Ville" },
                values: new object[] { "2 Avenue Client", "USER001", "3000", "user@livraison.local", "Client", "Demo", "70000002", "Sfax" });

            migrationBuilder.CreateIndex(
                name: "IX_Comptes_CIN",
                table: "Comptes",
                column: "CIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comptes_Email",
                table: "Comptes",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comptes_CIN",
                table: "Comptes");

            migrationBuilder.DropIndex(
                name: "IX_Comptes_Email",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "CIN",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "CodePostal",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Comptes");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "Comptes");
        }
    }
}
