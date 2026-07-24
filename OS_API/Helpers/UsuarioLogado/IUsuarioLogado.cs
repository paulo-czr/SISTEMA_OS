using System.Security.Claims;

namespace OS_API.Helpers.UsuarioLogado
{
    public interface IUsuarioLogado
    {
        bool Autenticado { get; }

        string? IdUsuario { get; }

        string? UserName { get; }

        string? Email { get; }

        string retornarUserLogado();
        Task<int> RetornarIdFuncionarioLogado();

        IEnumerable<Claim> Claims { get; }
    }
}
