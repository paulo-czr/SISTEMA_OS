using OS_API.DTOs.Cliente;
using OS_API.DTOs.Cliente.Filtro;
using OS_API.DTOs.OrdemServico.Filtro;
using OS_API.Models.Cliente;

namespace OS_API.Interfaces.Services
{
    public interface IClienteService
    {
        Task<ClienteDto> Criar(CriarClienteDto dto);

        Task<ClienteDto> Atualizar(int id, AtualizarClienteDto dto);

        Task<ClienteDto?> BuscarPorId(int id);

        Task<ClienteDto?> BuscarPorDocumento(string documento);

        Task<List<ClienteDto>> Listar();

        Task<ResultadoPaginadoClienteDto> ListarPaginado(FiltroClienteDto filtro);

        Task<ClienteModel> BuscarClienteOuFalhar(int id);

        Task Remover(int id);
    }
}