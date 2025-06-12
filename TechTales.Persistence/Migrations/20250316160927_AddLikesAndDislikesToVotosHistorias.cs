using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTales.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndDislikesToVotosHistorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContrasenaHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<string>(type: "enum('usuario','admin')", nullable: true, defaultValueSql: "'usuario'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "historias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contenido = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "historias_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "retos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdAdmin = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "retos_ibfk_1",
                        column: x => x.IdAdmin,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "votoshistorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdHistoria = table.Column<int>(type: "int", nullable: false),
                    Voto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    FechaVoto = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "votoshistorias_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "votoshistorias_ibfk_2",
                        column: x => x.IdHistoria,
                        principalTable: "historias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Contenido = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaComentario = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdHistoria = table.Column<int>(type: "int", nullable: true),
                    IdReto = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "comentarios_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "comentarios_ibfk_2",
                        column: x => x.IdHistoria,
                        principalTable: "historias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "comentarios_ibfk_3",
                        column: x => x.IdReto,
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "retoscompletados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdReto = table.Column<int>(type: "int", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreationUser = table.Column<int>(type: "int", nullable: false),
                    UserMod = table.Column<int>(type: "int", nullable: true),
                    UserDeleted = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "retoscompletados_ibfk_1",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "retoscompletados_ibfk_2",
                        column: x => x.IdReto,
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IdHistoria",
                table: "comentarios",
                column: "IdHistoria");

            migrationBuilder.CreateIndex(
                name: "IdReto",
                table: "comentarios",
                column: "IdReto");

            migrationBuilder.CreateIndex(
                name: "IdUsuario",
                table: "comentarios",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IdUsuario1",
                table: "historias",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IdAdmin",
                table: "retos",
                column: "IdAdmin");

            migrationBuilder.CreateIndex(
                name: "IdReto1",
                table: "retoscompletados",
                column: "IdReto");

            migrationBuilder.CreateIndex(
                name: "IdUsuario2",
                table: "retoscompletados",
                columns: new[] { "IdUsuario", "IdReto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IdHistoria1",
                table: "votoshistorias",
                column: "IdHistoria");

            migrationBuilder.CreateIndex(
                name: "IdUsuario3",
                table: "votoshistorias",
                columns: new[] { "IdUsuario", "IdHistoria" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comentarios");

            migrationBuilder.DropTable(
                name: "retoscompletados");

            migrationBuilder.DropTable(
                name: "votoshistorias");

            migrationBuilder.DropTable(
                name: "retos");

            migrationBuilder.DropTable(
                name: "historias");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
