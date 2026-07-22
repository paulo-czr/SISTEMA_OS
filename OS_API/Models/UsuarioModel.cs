using Microsoft.AspNetCore.Identity;

namespace OS_API.Models
{
    public class UsuarioModel : IdentityUser
    {
        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public FuncionarioModel? Funcionario { get; set; }
    }
}
