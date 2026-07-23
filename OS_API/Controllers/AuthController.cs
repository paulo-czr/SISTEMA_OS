using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OS_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OS_API.Helpers.Constantes;
using OS_API.DTOs.AuthDto;
using OS_API.Interfaces.Services;

namespace OS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<UsuarioModel> _userManager;

        public AuthController(IAuthService authService, UserManager<UsuarioModel> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthCreateDto dto)
        {
            var usuario = await _authService.Login(dto);
            return Ok(usuario);
        }
    }
}
