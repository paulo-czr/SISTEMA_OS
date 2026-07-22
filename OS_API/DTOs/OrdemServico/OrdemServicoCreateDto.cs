using OS_API.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.OrdemServico
{
    public class CriarOrdemServicoDto
    {
        [Required(ErrorMessage = "É obrigatório preencher o campo Título.")]
        public string TituloOs { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "É obrigatório informar o Tipo de Atendimento.")]
        public int IdTipoAtendimento { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o Cliente.")]
        public int IdCliente { get; set; }

        public StatusOs Status { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public DateOnly Prazo { get; set; }

        public string? RelatorioTecnico { get; set; }

        public string? Observacao { get; set; }

        public List<int> Funcionarios { get; set; } = new();
    }
}
