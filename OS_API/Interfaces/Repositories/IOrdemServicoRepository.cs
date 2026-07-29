using OS_API.Models;

namespace OS_API.Interfaces.Repositories
{
    public interface IOrdemServicoRepository
    {
        Task<OrdemServicoModel> Adicionar(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorId(int id);

        Task<List<OrdemServicoModel>> Listar();

        Task Atualizar(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorToken(string token);

        Task Remover(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorTipoAtendimento(TipoAtendimento tipo);

        Task<List<OrdemServicoModel>> BuscarPorIdUsuarioFuncionario(string idUsuario);

        Task<OrdemServicoModel?> BuscarPorTokenFotos(string token);
    }
}
