using OS_API.DTOs.Tecnico;

namespace OS_API.Interfaces.Services
{
    public interface IFuncionarioService
    {
        Task<FuncionarioDto> Criar(CriarFuncionarioDto dto);

        Task<FuncionarioDto?> BuscarPorId(int id);

        Task<List<FuncionarioDto>> Listar();

        Task<FuncionarioDto> Atualizar(int id, AtualizarFuncionarioDto dto);

        Task Remover(int id);
    }
}
