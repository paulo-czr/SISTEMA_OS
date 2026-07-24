using OS_API.DTOs.OrdemServico;

namespace OS_API.Interfaces.Services
{
    public interface IOrdemServicoService
    {
        Task<BuscarOrdemServicoDto> Criar(CriarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto> Atualizar(int id, AtualizarOrdemServicoDto dto);

        Task<BuscarOrdemServicoDto?> BuscarPorId(int id);

        Task<List<BuscarOrdemServicoDto>> Listar();

        // Método separado só pro funcionário preencher/editar o relatório dele,
        // sem precisar (nem poder) mexer nos outros campos da OS.
        Task<BuscarOrdemServicoDto> AtualizarRelatorio(int id, AtualizarRelatorioDto dto);

        // Método separado só pra mudar o status da OS.
        Task<BuscarOrdemServicoDto> AlterarStatus(int id, AlterarStatusOsDto dto);

        Task Remover(int id);
    }
}
