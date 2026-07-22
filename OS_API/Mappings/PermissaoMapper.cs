using OS_API.DTOs.Permissao;
using OS_API.Models;

namespace OS_API.Mappings
{
    public static class PermissaoMapper
    {
        public static PermissaoDto ParaDto(PermissaoModel model)
        {
            return new PermissaoDto
            {
                Id = model.Id,
                Nome = model.Nome,
                Descricao = model.Descricao,
                Modulo = model.Modulo
            };
        }
    }
}
