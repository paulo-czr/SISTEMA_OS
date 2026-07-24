using OS_API.DTOs.OSFuncionario;

namespace OS_API.Interfaces.Services
{
    public interface IOsFuncionarioService
    {
        Task AdicionarTecnicoAsync(int idOs, int idFuncionario, bool responsavel);

        Task RemoverTecnicoAsync(int idOsFuncionario);

        Task DefinirResponsavelAsync(int idOs, int idFuncionario);

        Task<List<OsFuncionarioDetalheDto>> ObterTecnicosDaOsAsync(int idOs);
    }
}
