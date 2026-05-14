using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientService.Migrations
{
    /// <inheritdoc />
    public partial class UserClientLinking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Clients",
                type: "nvarchar(180)",
                maxLength: 180,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CIN",
                table: "Clients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompteId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Clients",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Adresse", "CIN", "CompteId", "Email", "Telephone" },
                values: new object[] { "10 Rue de Marseille", "CL001", null, "amine.client@livraison.local", "71111111" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Adresse", "CIN", "CompteId", "Email", "Telephone" },
                values: new object[] { "25 Avenue Habib Bourguiba", "CL002", null, "sarra.client@livraison.local", "72222222" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Adresse", "CIN", "CompteId", "Email", "Telephone" },
                values: new object[] { "8 Rue des Oliviers", "CL003", null, "youssef.client@livraison.local", "73333333" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CIN",
                table: "Clients",
                column: "CIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompteId",
                table: "Clients",
                column: "CompteId",
                unique: true,
                filter: "[CompteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_CIN",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompteId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CIN",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompteId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Clients");
        }
    }
}
