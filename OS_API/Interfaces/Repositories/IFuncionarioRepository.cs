using OS_API.Models;

namespace OS_API.Interfaces.Repositories
{
    public interface IFuncionarioRepository
    {
        Task<FuncionarioModel> Adicionar(FuncionarioModel tecnico);

        Task<FuncionarioModel?> BuscarPorId(int id);

        Task<List<FuncionarioModel>> Listar();

        Task Atualizar(FuncionarioModel tecnico);

        Task Remover(FuncionarioModel tecnico);
    }
}
