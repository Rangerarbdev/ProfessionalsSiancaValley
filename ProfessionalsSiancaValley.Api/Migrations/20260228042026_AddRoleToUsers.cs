using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfessionalsSiancaValley.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MiniatureId_Miniature",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalReportes",
                table: "Miniatures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MiniatureId_Miniature",
                table: "Reports",
                column: "MiniatureId_Miniature");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Miniatures_MiniatureId_Miniature",
                table: "Reports",
                column: "MiniatureId_Miniature",
                principalTable: "Miniatures",
                principalColumn: "Id_Miniature");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Miniatures_MiniatureId_Miniature",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_MiniatureId_Miniature",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.DropColumn(
                name: "MiniatureId_Miniature",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TotalReportes",
                table: "Miniatures");
        }
    }
}
