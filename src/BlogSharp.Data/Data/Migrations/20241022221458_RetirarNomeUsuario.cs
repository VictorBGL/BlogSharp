using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSharp.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class RetirarNomeUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
