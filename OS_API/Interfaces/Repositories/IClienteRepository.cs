using OS_API.DTOs.Cliente.Filtro;
using OS_API.DTOs.OrdemServico.Filtro;
using OS_API.Models;
using OS_API.Models.Cliente;

namespace OS_API.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task<ClienteModel> Adicionar(ClienteModel cliente);

        Task<ClienteModel?> BuscarPorId(int id);

        Task<ClienteModel?> BuscarPorDocumento(string documento);

        Task<bool> ExisteDocumento(string documento);

        Task<bool> ExisteDocumentoEmOutroCliente(string documento, int idCliente);

        Task<bool> ExisteEmailEmOutroCliente(string email, int idCliente);

        Task<List<ClienteModel>> Listar();

        Task<(List<ClienteModel> Itens, int Total)> ListarPaginado(FiltroClienteDto filtro);

        Task Atualizar(ClienteModel cliente);

        Task Remover(ClienteModel cliente);
    }
}