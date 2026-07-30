using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class fotosPdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ArquivoPdfFotos",
                table: "OrdensServico",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenFotos",
                table: "OrdensServico",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenFotosExpiraEm",
                table: "OrdensServico",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArquivoPdfFotos",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "TokenFotos",
                table: "OrdensServico");

            migrationBuilder.DropColumn(
                name: "TokenFotosExpiraEm",
                table: "OrdensServico");
        }
    }
}
