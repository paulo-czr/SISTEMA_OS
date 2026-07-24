using OS_API.DTOs.Permissao;
using OS_API.DTOs.Usuario;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IPermissaoRepository _permissaoRepository;

        public UsuarioService(IUsuarioRepository repository, IPermissaoRepository permissaoRepository)
        {
            _repository = repository;
            _permissaoRepository = permissaoRepository;
        }

        public async Task AdicionarPermissaoPorTipoUsuario(UsuarioModel usuario, TipoUsuario tipo)
        {
            await _repository.AdicionarPermissaoPorTipoUser(usuario, tipo);
        }

        public async Task<UsuarioModel> Criar(UsuarioModel usuario, string senha)
        {
            //verificar se ja tem email
            var userEmail = await _repository.BuscarPeloEmail(usuario.Email);
            if (userEmail != null)
            {
                throw new ValidacaoException("Email ja cadastrado");
            }
            //verificar se ja tem usuario
            var userName = await _repository.BuscarPeloUserName(usuario.UserName);
            if (userName != null)
            {
                throw new ValidacaoException("user name ja cadastrado");
            }

            return await _repository.Criar(usuario, senha);
        }

        public async Task<UsuarioDto> BuscarPorId(string id)
        {
            var usuario = await BuscarOuFalhar(id);

            return UsuarioMapper.ParaUsuarioDto(usuario);
        }

        public async Task<List<UsuarioDto>> Listar()
        {
            var usuarios = await _repository.Listar();

            return usuarios
                .Select(UsuarioMapper.ParaUsuarioDto)
                .ToList();
        }

        public async Task<UsuarioDto> Atualizar(string id, AtualizarUsuarioDto dto)
        {
            var usuario = await BuscarOuFalhar(id);

            // Validar se o novo UserName já pertence a outro usuário.
            var usuarioComMesmoUserName = await _repository.BuscarPeloUserName(dto.UserName);
            if (usuarioComMesmoUserName != null && usuarioComMesmoUserName.Id != usuario.Id)
                throw new ValidacaoException("user name ja cadastrado");

            // Validar se o novo Email já pertence a outro usuário.
            var usuarioComMesmoEmail = await _repository.BuscarPeloEmail(dto.Email);
            if (usuarioComMesmoEmail != null && usuarioComMesmoEmail.Id != usuario.Id)
                throw new ValidacaoException("Email ja cadastrado");

            UsuarioMapper.AtualizarModel(usuario, dto);

            await _repository.Atualizar(usuario);

            return UsuarioMapper.ParaUsuarioDto(usuario);
        }

        public async Task Remover(string id)
        {
            var usuario = await BuscarOuFalhar(id);
            await _repository.Remover(usuario);
        }

        public async Task<List<PermissaoDto>> ListarPermissoes(string id)
        {
            var usuario = await BuscarOuFalhar(id);

            // Nomes das permissões (claims) que esse usuário
            var nomesVinculados = await _repository.BuscarPermissoes(usuario);

            var todasPermissoes = await _permissaoRepository.Listar();

            return todasPermissoes
                .Where(p => nomesVinculados.Contains(p.Nome))
                .Select(PermissaoMapper.ParaDto)
                .ToList();
        }

        public async Task<List<PermissaoDto>> AtualizarPermissoes(string id, List<int> idsPermissao)
        {
            var usuario = await BuscarOuFalhar(id);

            var todasPermissoes = await _permissaoRepository.Listar();

            // Confere se todos os Ids enviados realmente existem na tabela Permissao.
            var idsInvalidos = idsPermissao
                .Except(todasPermissoes.Select(p => p.Id))
                .ToList();

            if (idsInvalidos.Any())
                throw new EntidadeNaoEncontradaException(
                    $"Permissão(ões) não encontrada(s): {string.Join(", ", idsInvalidos)}");

            // Validar se a lista final não deixa o usuário sem nenhuma permissão essencial
            // para o seu TipoUsuario/Role.

            var permissoesSelecionadas = todasPermissoes
                .Where(p => idsPermissao.Contains(p.Id))
                .ToList();

            var nomes = permissoesSelecionadas.Select(p => p.Nome).ToList();

            await _repository.SincronizarPermissoes(usuario, nomes);

            return permissoesSelecionadas
                .Select(PermissaoMapper.ParaDto)
                .ToList();
        }

        private async Task<UsuarioModel> BuscarOuFalhar(string id)
        {
            var usuario = await _repository.BuscarPorId(id);

            if (usuario == null)
                throw new EntidadeNaoEncontradaException("Usuário não encontrado.");

            return usuario;
        }
    }
}
