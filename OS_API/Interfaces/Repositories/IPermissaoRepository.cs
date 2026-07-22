using OS_API.Models;

namespace OS_API.Interfaces.Repositories
{
    public interface IPermissaoRepository
    {
        Task<PermissaoModel?> BuscarPorId(int id);

        Task<PermissaoModel?> BuscarPorNome(string nome);

        Task<List<PermissaoModel>> Listar();
    }
}
