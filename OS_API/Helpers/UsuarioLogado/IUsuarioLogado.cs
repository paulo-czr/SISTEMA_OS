using System.Security.Claims;

namespace OS_API.Helpers.UsuarioLogado
{
    public interface IUsuarioLogado
    {
        bool Autenticado { get; }

        string? IdUsuario { get; }

        string? UserName { get; }

        string? Email { get; }

        IEnumerable<Claim> Claims { get; }
    }
}
