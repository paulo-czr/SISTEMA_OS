using OS_API.DTOs.OrdemServico;

namespace OS_API.Interfaces.Services
{
    public interface IOrdemServicoService
    {
        Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto> Atualizar(int id, AtualizarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto?> BuscarPorId(int id);

        Task<List<BuscarOrdemServicoDto>> Listar();

        Task Remover(int id);
    }
}
