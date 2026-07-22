using OS_API.DTOs.Tecnico;
using OS_API.Models;

namespace OS_API.Mappings
{
    public static class FuncionarioMapper
    {
        public static FuncionarioModel ParaModel(CriarFuncionarioDto dto, string usuarioId)
        {
            return new FuncionarioModel(
                dto.Nome,
                usuarioId
            );
        }

        public static FuncionarioDto ParaDto(FuncionarioModel model)
        {
            return new FuncionarioDto
            {
                Id = model.Id,
                Nome = model.Nome,
                UsuarioId = model.UsuarioId,
                UserName = model.Usuario?.UserName ?? string.Empty,
                Email = model.Usuario?.Email ?? string.Empty,
                Ativo = model.Usuario?.Ativo ?? false
            };
        }
    }
}
