using OS_API.DTOs.TipoAtendimento;
using OS_API.Models;

namespace OS_API.Interfaces.Services
{
    public interface ITipoAtendimentoService
    {
        Task<TipoAtendimentoDto> Criar(CriarTipoAtendimentoDto dto);

        Task<TipoAtendimentoDto> Atualizar(int id, AtualizarTipoAtendimentoDto dto);

        Task<TipoAtendimentoDto?> BuscarPorId(int id);

        Task<TipoAtendimento> BuscarOuFalhar(int id);

        Task<List<TipoAtendimentoDto>> Listar();

        Task Remover(int id);
    }
}
