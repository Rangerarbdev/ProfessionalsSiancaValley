using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProfessionalsSiancaValley.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Miniatures",
                columns: table => new
                {
                    Id_Miniature = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Publicacion = table.Column<int>(type: "integer", nullable: false),
                    Id_User = table.Column<string>(type: "text", nullable: false),
                    Nombre_Usuario = table.Column<string>(type: "text", nullable: false),
                    Email_Usuario = table.Column<string>(type: "text", nullable: false),
                    Tipo_Contenido = table.Column<string>(type: "text", nullable: false),
                    Url_Miniatura = table.Column<string>(type: "text", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Fecha_Publicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Vistas = table.Column<int>(type: "integer", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Dislikes = table.Column<int>(type: "integer", nullable: false),
                    Es_Contenido_Sensible = table.Column<bool>(type: "boolean", nullable: false),
                    Bloqueado_Por_Sistema = table.Column<bool>(type: "boolean", nullable: false),
                    Estado_Revision = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miniatures", x => x.Id_Miniature);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id_user = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    user_position = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dni = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado_edad = table.Column<bool>(type: "boolean", nullable: false),
                    professional_license = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    specialty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    university = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    professional_association_registration = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id_Report = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Miniature = table.Column<int>(type: "integer", nullable: false),
                    Id_User = table.Column<string>(type: "text", nullable: false),
                    Nombre_Usuario = table.Column<string>(type: "text", nullable: false),
                    Email_Usuario = table.Column<string>(type: "text", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Fecha_Reporte = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id_Report);
                    table.ForeignKey(
                        name: "FK_Reports_Miniatures_Id_Miniature",
                        column: x => x.Id_Miniature,
                        principalTable: "Miniatures",
                        principalColumn: "Id_Miniature",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Id_Miniature",
                table: "Reports",
                column: "Id_Miniature");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Miniatures");
        }
    }
}
