using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class excluir_ipagenteosModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Assinaturas");

            migrationBuilder.DropColumn(
                name: "UserAgente",
                table: "Assinaturas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Assinaturas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAgente",
                table: "Assinaturas",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
