using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class usuarioOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdUsuarioQueRegistrou",
                table: "OrdensServico",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_IdUsuarioQueRegistrou",
                table: "OrdensServico",
                column: "IdUsuarioQueRegistrou");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdensServico_Usuarios_IdUsuarioQueRegistrou",
                table: "OrdensServico",
                column: "IdUsuarioQueRegistrou",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdensServico_Usuarios_IdUsuarioQueRegistrou",
                table: "OrdensServico");

            migrationBuilder.DropIndex(
                name: "IX_OrdensServico_IdUsuarioQueRegistrou",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "IdUsuarioQueRegistrou",
                table: "OrdensServico");
        }
    }
}
