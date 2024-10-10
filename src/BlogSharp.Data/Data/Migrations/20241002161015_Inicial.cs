using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSharp.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Administrador = table.Column<bool>(type: "bit", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PublicacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Publicacoes_PublicacaoId",
                        column: x => x.PublicacaoId,
                        principalTable: "Publicacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_AutorId",
                table: "Comentarios",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PublicacaoId",
                table: "Comentarios",
                column: "PublicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_AutorId",
                table: "Publicacoes",
                column: "AutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
