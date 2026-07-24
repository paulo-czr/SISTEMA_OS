using OS_API.DTOs.TipoAtendimento;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Models;

namespace OS_API.Services
{
    public class TipoAtendimentoService : ITipoAtendimentoService
    {
        private readonly ITipoAtendimentoRepository _repository;

        public TipoAtendimentoService(ITipoAtendimentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<TipoAtendimentoDto> Criar(CriarTipoAtendimentoDto dto)
        {
            var descricaoNormalizada = dto.Descricao.Trim();

            // Não deixa cadastrar dois Tipos de Atendimento com a mesma descrição.
            if (await _repository.ExisteDescricao(descricaoNormalizada))
                throw new ConflitoException("Já existe um Tipo de Atendimento com essa descrição.");

            var tipoAtendimento = TipoAtendimentoMapper.ParaModel(dto);

            tipoAtendimento = await _repository.Adicionar(tipoAtendimento);

            return TipoAtendimentoMapper.ParaDto(tipoAtendimento);
        }

        public async Task<TipoAtendimentoDto> Atualizar(int id, AtualizarTipoAtendimentoDto dto)
        {
            var tipoAtendimento = await BuscarOuFalhar(id);

            var descricaoNormalizada = dto.Descricao.Trim();

            // Só consulta o banco por duplicidade se a descrição realmente estiver mudando.
            if (descricaoNormalizada != tipoAtendimento.Descricao)
            {
                if (await _repository.ExisteDescricaoEmOutro(descricaoNormalizada, id))
                    throw new ConflitoException("Já existe outro Tipo de Atendimento com essa descrição.");
            }

            tipoAtendimento.Descricao = descricaoNormalizada;

            await _repository.Atualizar(tipoAtendimento);

            return TipoAtendimentoMapper.ParaDto(tipoAtendimento);
        }

        public async Task<TipoAtendimentoDto?> BuscarPorId(int id)
        {
            var tipoAtendimento = await BuscarOuFalhar(id);

            return TipoAtendimentoMapper.ParaDto(tipoAtendimento);
        }

        public async Task<List<TipoAtendimentoDto>> Listar()
        {
            var tipos = await _repository.Listar();

            return tipos
                .Select(TipoAtendimentoMapper.ParaDto)
                .ToList();
        }

        public async Task Remover(int id)
        {
            var tipoAtendimento = await BuscarOuFalhar(id);

            // Se um dia precisar impedir a remoção de um tipo já usado em alguma OS, validar aqui.

            await _repository.Remover(tipoAtendimento);
        }

        public async Task<TipoAtendimento> BuscarOuFalhar(int id)
        {
            var tipoAtendimento = await _repository.BuscarPorId(id);

            if (tipoAtendimento == null)
                throw new EntidadeNaoEncontradaException("Tipo de Atendimento não encontrado.");

            return tipoAtendimento;
        }
    }
}
