using OS_API.DTOs.OSFuncionario;
using OS_API.Models;

namespace OS_API.Mappings
{
    public static class OsFuncionarioMapper
    {
        public static OsFuncionarioDetalheDto ParaDto(OsFuncionarioModel model)
        {
            return new OsFuncionarioDetalheDto
            {
                IdOsFuncionario = model.IdOsFuncionario,
                IdFuncionario = model.IdFuncionario,
                NomeFuncionario = model.funcionario?.Nome ?? string.Empty,
                Responsavel = model.Responsavel
            };
        }
    }
}
