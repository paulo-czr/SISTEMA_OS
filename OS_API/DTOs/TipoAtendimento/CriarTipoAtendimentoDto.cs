using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.TipoAtendimento
{
    public class CriarTipoAtendimentoDto
    {
        [Required(ErrorMessage = "É obrigatório preencher a Descrição.")]
        public string Descricao { get; set; } = string.Empty;
    }
}
