using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.OrdemServico
{
    public class AtualizarRelatorioDto
    {
        [Required(ErrorMessage = "É obrigatório preencher o Relatório Técnico.")]
        public string RelatorioTecnico { get; set; } = string.Empty;

    }
}
