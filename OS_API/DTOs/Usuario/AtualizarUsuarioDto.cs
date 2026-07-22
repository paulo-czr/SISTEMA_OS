using OS_API.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Usuario
{
    /// <summary>
    /// DTO específico para atualização de usuário.
    /// Propositalmente não expõe Id (vem da rota) nem Senha — a troca de senha deve
    /// ter uma rota própria, já que exige regras diferentes (confirmar senha atual, etc.).
    /// </summary>
    public class AtualizarUsuarioDto
    {
        [Required(ErrorMessage = "É obrigatório preencher o campo userName.")]
        public string UserName { get; set; } = string.Empty;

        [EmailValido]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}
