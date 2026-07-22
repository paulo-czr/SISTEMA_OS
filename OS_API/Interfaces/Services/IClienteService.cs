using OS_API.DTOs.Cliente;

namespace OS_API.Interfaces.Services
{
    public interface IClienteService
    {
        Task<ClienteDto> Criar(CriarClienteDto dto);

        Task<ClienteDto> Atualizar(int id, AtualizarClienteDto dto);

        Task<ClienteDto?> BuscarPorId(int id);

        Task<ClienteDto?> BuscarPorDocumento(string documento);

        Task<List<ClienteDto>> Listar();

        Task Remover(int id);
    }
}