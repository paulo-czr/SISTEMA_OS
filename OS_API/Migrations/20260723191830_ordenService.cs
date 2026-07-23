using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OS_API.Migrations
{
    /// <inheritdoc />
    public partial class ordenService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoAtendimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoAtendimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdensServico",
                columns: table => new
                {
                    IdOs = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TituloOs = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdTipoAtendimento = table.Column<int>(type: "integer", nullable: false),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataHoraFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Prazo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RelatorioTecnico = table.Column<string>(type: "text", nullable: true),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    ArquivoPdf = table.Column<byte[]>(type: "bytea", nullable: false),
                    CogigoPdf = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensServico", x => x.IdOs);
                    table.ForeignKey(
                        name: "FK_OrdensServico_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdensServico_TipoAtendimento_IdTipoAtendimento",
                        column: x => x.IdTipoAtendimento,
                        principalTable: "TipoAtendimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssinaturaOsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdOs = table.Column<int>(type: "integer", nullable: false),
                    OrdemServicoIdOs = table.Column<int>(type: "integer", nullable: false),
                    NomeSignatario = table.Column<string>(type: "text", nullable: false),
                    DocumentoSignatario = table.Column<string>(type: "text", nullable: false),
                    ImagemAssinatura = table.Column<string>(type: "text", nullable: false),
                    DataAssinatura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    UserAgente = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssinaturaOsModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssinaturaOsModel_OrdensServico_OrdemServicoIdOs",
                        column: x => x.OrdemServicoIdOs,
                        principalTable: "OrdensServico",
                        principalColumn: "IdOs",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OsFuncionarios",
                columns: table => new
                {
                    IdOsFuncionario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdOs = table.Column<int>(type: "integer", nullable: false),
                    IdFuncionario = table.Column<int>(type: "integer", nullable: false),
                    Responsavel = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsFuncionarios", x => x.IdOsFuncionario);
                    table.ForeignKey(
                        name: "FK_OsFuncionarios_Funcionarios_IdFuncionario",
                        column: x => x.IdFuncionario,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OsFuncionarios_OrdensServico_IdOs",
                        column: x => x.IdOs,
                        principalTable: "OrdensServico",
                        principalColumn: "IdOs",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssinaturaOsModel_OrdemServicoIdOs",
                table: "AssinaturaOsModel",
                column: "OrdemServicoIdOs");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_IdCliente",
                table: "OrdensServico",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_IdTipoAtendimento",
                table: "OrdensServico",
                column: "IdTipoAtendimento");

            migrationBuilder.CreateIndex(
                name: "IX_OsFuncionarios_IdFuncionario",
                table: "OsFuncionarios",
                column: "IdFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_OsFuncionarios_IdOs_IdFuncionario",
                table: "OsFuncionarios",
                columns: new[] { "IdOs", "IdFuncionario" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssinaturaOsModel");

            migrationBuilder.DropTable(
                name: "OsFuncionarios");

            migrationBuilder.DropTable(
                name: "OrdensServico");

            migrationBuilder.DropTable(
                name: "TipoAtendimento");
        }
    }
}
