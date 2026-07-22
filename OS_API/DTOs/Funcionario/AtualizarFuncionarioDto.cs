using OS_API.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Tecnico
{
    /// <summary>
    /// Atualiza tanto os dados do Funcionario (Nome) quanto do Usuario vinculado
    /// (UserName, Email, Ativo). Troca de senha deve ter uma rota própria.
    /// </summary>
    public class AtualizarFuncionarioDto
    {
        [Required(ErrorMessage = "É obrigatório preencher o campo Nome.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "É obrigatório preencher o campo userName.")]
        public string UserName { get; set; } = string.Empty;

        [EmailValido]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}
