using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class assinatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssinaturaOsModel_OrdensServico_OrdemServicoIdOs",
                table: "AssinaturaOsModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssinaturaOsModel",
                table: "AssinaturaOsModel");

            migrationBuilder.RenameTable(
                name: "AssinaturaOsModel",
                newName: "Assinaturas");

            migrationBuilder.RenameIndex(
                name: "IX_AssinaturaOsModel_OrdemServicoIdOs",
                table: "Assinaturas",
                newName: "IX_Assinaturas_OrdemServicoIdOs");

            migrationBuilder.AddColumn<string>(
                name: "TokenAssinaturaCliente",
                table: "OrdensServico",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenAssinaturaExpiraEm",
                table: "OrdensServico",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssinaturaPadrao",
                table: "Funcionarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Assinaturas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assinaturas",
                table: "Assinaturas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoIdOs",
                table: "Assinaturas",
                column: "OrdemServicoIdOs",
                principalTable: "OrdensServico",
                principalColumn: "IdOs",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assinaturas_OrdensServico_OrdemServicoIdOs",
                table: "Assinaturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assinaturas",
                table: "Assinaturas");

            migrationBuilder.DropColumn(
                name: "TokenAssinaturaCliente",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "TokenAssinaturaExpiraEm",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "AssinaturaPadrao",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Assinaturas");

            migrationBuilder.RenameTable(
                name: "Assinaturas",
                newName: "AssinaturaOsModel");

            migrationBuilder.RenameIndex(
                name: "IX_Assinaturas_OrdemServicoIdOs",
                table: "AssinaturaOsModel",
                newName: "IX_AssinaturaOsModel_OrdemServicoIdOs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssinaturaOsModel",
                table: "AssinaturaOsModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssinaturaOsModel_OrdensServico_OrdemServicoIdOs",
                table: "AssinaturaOsModel",
                column: "OrdemServicoIdOs",
                principalTable: "OrdensServico",
                principalColumn: "IdOs",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
