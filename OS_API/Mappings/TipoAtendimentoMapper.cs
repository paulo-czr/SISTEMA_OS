using OS_API.DTOs.TipoAtendimento;
using OS_API.Models;

namespace OS_API.Mappings
{
    public static class TipoAtendimentoMapper
    {
        public static TipoAtendimento ParaModel(CriarTipoAtendimentoDto dto)
        {
            return new TipoAtendimento
            {
                Descricao = dto.Descricao.Trim()
            };
        }

        public static TipoAtendimentoDto ParaDto(TipoAtendimento model)
        {
            return new TipoAtendimentoDto
            {
                Id = model.Id,
                Descricao = model.Descricao ?? string.Empty
            };
        }
    }
}
