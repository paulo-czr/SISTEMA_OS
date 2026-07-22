using OS_API.Models.Enum;
using OS_API.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Tecnico
{
    public class CriarFuncionarioDto
    {
        [Required(ErrorMessage = "É obrigatório preencher o campo Nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "É obrigatório preencher o campo userName.")]
        public string UserName { get; set; } = string.Empty;

        [EmailValido]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "É obrigatório preencher o campo senha.")]
        public string Senha { get; set; } = string.Empty;

        //Só pode ser 1,2,3
        public TipoUsuario TipoUsuario { get; set; }
    }
}
