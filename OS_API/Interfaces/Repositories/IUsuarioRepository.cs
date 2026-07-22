using Microsoft.AspNetCore.Identity;
using OS_API.Models;
using OS_API.Models.Enum;
using System.Security.Claims;

namespace OS_API.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioModel> Criar(UsuarioModel usuario, string senha);

        Task<UsuarioModel?> BuscarPorId(string id);

        Task<List<UsuarioModel>> Listar();

        Task Atualizar(UsuarioModel usuario);

        Task Remover(UsuarioModel usuario);

        Task<UsuarioModel> BuscarPeloUserEmail(string usuario);
        Task<UsuarioModel> BuscarPeloEmail(string usuario);
        Task<UsuarioModel> BuscarPeloUserName(string usuario);
        Task<Boolean> ValidarSenha(UsuarioModel usuario, string senha);
        Task<IList<Claim>> BuscarClaims(UsuarioModel usuario);
        Task AdicionarPermissaoPorTipoUser(UsuarioModel usuario, TipoUsuario tipo);

        // Permissões individuais (fora do pacote padrão do TipoUsuario)
        Task<List<string>> BuscarPermissoes(UsuarioModel usuario);
        Task SincronizarPermissoes(UsuarioModel usuario, List<string> nomesPermissoes);
    }
}
