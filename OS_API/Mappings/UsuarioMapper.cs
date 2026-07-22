using OS_API.DTOs.AuthDto;
using OS_API.DTOs.Usuario;
using OS_API.Models;

namespace OS_API.Mappings
{
    public static class UsuarioMapper
    {
        public static AuthDto ParaDto(UsuarioModel model, string token)
        {
            return new AuthDto
            {
                Id = model.Id,
                Usuario = model.UserName!,
                Email = model.Email!,
                Token = token
            };
        }

        public static UsuarioDto ParaUsuarioDto(UsuarioModel model)
        {
            return new UsuarioDto
            {
                Id = model.Id,
                UserName = model.UserName!,
                Email = model.Email!,
                Ativo = model.Ativo,
                DataCadastro = model.DataCadastro
            };
        }

        public static void AtualizarModel(UsuarioModel model, AtualizarUsuarioDto dto)
        {
            model.UserName = dto.UserName;
            model.Email = dto.Email;
            model.Ativo = dto.Ativo;
        }
    }
}
