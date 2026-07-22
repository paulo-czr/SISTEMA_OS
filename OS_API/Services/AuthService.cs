using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OS_API.DTOs.AuthDto;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OS_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _repositorio;
        private readonly IConfiguration _configuration;
        public AuthService(IUsuarioRepository repository, IConfiguration configuration) 
        {
            _repositorio = repository;
            _configuration = configuration;
        }
        public async Task<AuthDto> Login(AuthCreateDto dto)
        {
            // Busca usuário
            var usuario = await _repositorio.BuscarPeloUserEmail(dto.usuario);
            //ver se existe
            if (usuario == null)
            {
                throw new EntidadeNaoEncontradaException("Usuario invalido");
            }
            // Confere a senha
            if (!await _repositorio.ValidarSenha(usuario, dto.Senha))
            {
                throw new EntidadeNaoEncontradaException("Senha invalida");
            }

            // Claims básicas do JWT
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Name, usuario.UserName!)
            };

            // Busca as claims cadastradas no Identity
            var claimsUsuario = await _repositorio.BuscarClaims(usuario);

            // Adiciona ao token
            claims.AddRange(claimsUsuario);


            // Chave do token
            var chave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                ));


            var credencial = new SigningCredentials(
                chave,
                SecurityAlgorithms.HmacSha256);


            // Criar token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credencial
            );

            // Converte o token para string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return UsuarioMapper.ParaDto(usuario, tokenString);
        }
    }
}
