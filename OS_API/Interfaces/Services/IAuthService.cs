using OS_API.DTOs.AuthDto;

namespace OS_API.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthDto> Login(AuthCreateDto dto);
    }
}
