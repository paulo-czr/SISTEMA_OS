using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.DTOs.OrdemServico
{
    public class BuscarOrdemServicoDto
    {
        public int IdOs { get; set; }

        public string TituloOs { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public int IdTipoAtendimento { get; set; }

        public string NomeTipoAtendimento { get; set; } = string.Empty;

        public int IdCliente { get; set; }

        public string NomeCliente { get; set; } = string.Empty;

        public StatusOs Status { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public DateOnly Prazo { get; set; }

        public string? RelatorioTecnico { get; set; }

        public string? Observacao { get; set; }

        public string CogigoPdf { get; set; } = string.Empty;

        public List<int> Funcionarios { get; set; } = new();
    }

}
