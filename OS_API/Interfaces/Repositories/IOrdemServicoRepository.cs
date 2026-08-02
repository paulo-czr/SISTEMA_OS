using OS_API.DTOs.OrdemServico.Filtro;
using OS_API.Models;
using OS_API.Models.Cliente;

namespace OS_API.Interfaces.Repositories
{
    public interface IOrdemServicoRepository
    {
        Task<OrdemServicoModel> Adicionar(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorId(int id);

        Task<List<OrdemServicoModel>> Listar();

        Task<(List<OrdemServicoModel> Itens, int Total)> ListarPaginado(FiltroOrdemServicoDto filtro, string? idUsuarioFuncionario);
        Task Atualizar(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorToken(string token);

        Task Remover(OrdemServicoModel ordemServico);

        Task<OrdemServicoModel?> BuscarPorTipoAtendimento(TipoAtendimento tipo);

        Task<OrdemServicoModel?> BuscarPorCliente(ClienteModel cliente);

        Task<List<OrdemServicoModel>> BuscarPorIdUsuarioFuncionario(string idUsuario);

        Task<OrdemServicoModel?> BuscarPorTokenFotos(string token);
    }
}
