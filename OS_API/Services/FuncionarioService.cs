using Microsoft.EntityFrameworkCore;
using OS_API.DTOs.Tecnico;
using OS_API.DTOs.Usuario;
using OS_API.Exceptionn;
using OS_API.Interfaces.Repositories;
using OS_API.Interfaces.Services;
using OS_API.Mappings;
using OS_API.Models;
using OS_API.Repositories;

namespace OS_API.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;
        private readonly IUsuarioService _usuarioService;
        private readonly IUnidadeTrabalho _unidadeTrabalho;

        public FuncionarioService(IFuncionarioRepository repository,
            IUsuarioService usuarioService,
            IUnidadeTrabalho unidadeTrabalho)
        {
            _repository = repository;
            _usuarioService = usuarioService;
            _unidadeTrabalho = unidadeTrabalho;
        }

        public async Task<FuncionarioDto> Criar(CriarFuncionarioDto dto)
        {
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();
                //cadastrar usuario primeiro
                var usuario = new UsuarioModel
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                };
                var usuarioBanco = await _usuarioService.Criar(usuario, dto.Senha);
                var fun = FuncionarioMapper.ParaModel(dto, usuarioBanco.Id);
                fun = await _repository.Adicionar(fun);

                //criar a permissao de acordo com o tipo
                await _usuarioService.AdicionarPermissaoPorTipoUsuario(usuarioBanco, dto.TipoUsuario);

                await _unidadeTrabalho.ConfirmarTransacaoAsync();

                return FuncionarioMapper.ParaDto(fun);
            }
            catch
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
        }

        public async Task<FuncionarioDto?> BuscarPorId(int id)
        {
            var tecnico = await _repository.BuscarPorId(id);

            if (tecnico == null)
            {
                throw new EntidadeNaoEncontradaException("Técnico não encontrado.");
            }

            return FuncionarioMapper.ParaDto(tecnico);
        }

        public async Task<List<FuncionarioDto>> Listar()
        {
            var tecnicos = await _repository.Listar();

            return tecnicos
                .Select(FuncionarioMapper.ParaDto)
                .ToList();
        }

        //public async Task Atualizar(int id, AtualizarTecnicoDto dto)
        //{
        //    var tecnico = await _repository.BuscarPorId(id);

        //    if (tecnico == null)
        //        throw new Exception("Técnico não encontrado.");

        //    tecnico.Atualizar(dto.Nome, dto.Telefone, dto.Email);
        //    tecnico.AlterarStatus(dto.Ativo);

        //    await _repository.Atualizar(tecnico);
        //}

        public async Task<FuncionarioDto> Atualizar(int id, AtualizarFuncionarioDto dto)
        {
            var funcionario = await _repository.BuscarPorId(id);

            if (funcionario == null)
                throw new EntidadeNaoEncontradaException("Funcionário não encontrado.");

            // Validar se o novo UserName/Email já pertence a outro usuário
            // (essa checagem já acontece dentro de _usuarioService.Atualizar).

            funcionario.AtualizarNome(dto.Nome);
            await _repository.Atualizar(funcionario);

            // Funcionario "é" um Usuario: os dados de conta (UserName/Email/Ativo)
            // são atualizados através do UsuarioService, reaproveitando as validações
            // que já existem lá.
            var atualizarUsuarioDto = new AtualizarUsuarioDto
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Ativo = dto.Ativo
            };
            await _usuarioService.Atualizar(funcionario.UsuarioId, atualizarUsuarioDto);

            var funcionarioAtualizado = await _repository.BuscarPorId(id);

            return FuncionarioMapper.ParaDto(funcionarioAtualizado!);
        }

        public async Task Remover(int id)
        {
            var tecnico = await _repository.BuscarPorId(id);

            if (tecnico == null)
                throw new Exception("Técnico não encontrado.");

            await _repository.Remover(tecnico);
        }
    }

}
