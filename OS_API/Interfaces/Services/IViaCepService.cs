using OS_API.DTOs.ViaCepDto;

namespace OS_API.Interfaces.Services
{
    public interface IViaCepService
    {
        Task<ViaCepDto?> ObterEnderecoPorCepAsync(string cep);
    }
}
