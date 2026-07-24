using OS_API.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.OrdemServico
{
    public class AlterarStatusOsDto
    {
        [Required(ErrorMessage = "É obrigatório informar o Status.")]
        public StatusOs Status { get; set; }
    }
}
