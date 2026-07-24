using OS_API.Models;

namespace OS_API.Interfaces.Repositories
{
    public interface ITipoAtendimentoRepository
    {
        Task<TipoAtendimento> Adicionar(TipoAtendimento tipoAtendimento);

        Task<TipoAtendimento?> BuscarPorId(int id);

        Task<bool> ExisteDescricao(string descricao);

        Task<bool> ExisteDescricaoEmOutro(string descricao, int id);

        Task<List<TipoAtendimento>> Listar();

        Task Atualizar(TipoAtendimento tipoAtendimento);

        Task Remover(TipoAtendimento tipoAtendimento);
    }
}
