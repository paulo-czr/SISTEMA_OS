using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class inconsistencia_assinatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoIdOs",
                table: "Assinaturas");

            migrationBuilder.DropIndex(
                name: "IX_Assinaturas_OrdemServicoIdOs",
                table: "Assinaturas");

            migrationBuilder.DropColumn(
                name: "OrdemServicoIdOs",
                table: "Assinaturas");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgente",
                table: "Assinaturas",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NomeSignatario",
                table: "Assinaturas",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Assinaturas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentoSignatario",
                table: "Assinaturas",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "OrdemServicoModelIdOs",
                table: "Assinaturas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assinaturas_IdOs",
                table: "Assinaturas",
                column: "IdOs");

            migrationBuilder.CreateIndex(
                name: "IX_Assinaturas_OrdemServicoModelIdOs",
                table: "Assinaturas",
                column: "OrdemServicoModelIdOs");

            migrationBuilder.AddForeignKey(
                name: "FK_Assinaturas_OrdensServico_IdOs",
                table: "Assinaturas",
                column: "IdOs",
                principalTable: "OrdensServico",
                principalColumn: "IdOs",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoModelIdOs",
                table: "Assinaturas",
                column: "OrdemServicoModelIdOs",
                principalTable: "OrdensServico",
                principalColumn: "IdOs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assinaturas_OrdensServico_IdOs",
                table: "Assinaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoModelIdOs",
                table: "Assinaturas");

            migrationBuilder.DropIndex(
                name: "IX_Assinaturas_IdOs",
                table: "Assinaturas");

            migrationBuilder.DropIndex(
                name: "IX_Assinaturas_OrdemServicoModelIdOs",
                table: "Assinaturas");

            migrationBuilder.DropColumn(
                name: "OrdemServicoModelIdOs",
                table: "Assinaturas");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgente",
                table: "Assinaturas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "NomeSignatario",
                table: "Assinaturas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Assinaturas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentoSignatario",
                table: "Assinaturas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "OrdemServicoIdOs",
                table: "Assinaturas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assinaturas_OrdemServicoIdOs",
                table: "Assinaturas",
                column: "OrdemServicoIdOs");

            migrationBuilder.AddForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoIdOs",
                table: "Assinaturas",
                column: "OrdemServicoIdOs",
                principalTable: "OrdensServico",
                principalColumn: "IdOs",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
