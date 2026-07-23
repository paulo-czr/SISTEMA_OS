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
            return FuncionarioMapper.ParaDto(tecnico);
        }

        public async Task<List<FuncionarioDto>> Listar()
        {
            var tecnicos = await _repository.Listar();

            return tecnicos
                .Select(FuncionarioMapper.ParaDto)
                .ToList();
        }



        public async Task<FuncionarioDto> Atualizar(int id, AtualizarFuncionarioDto dto)
        {
            var funcionario = await _repository.BuscarPorId(id);

            if (funcionario == null)
                throw new EntidadeNaoEncontradaException("Funcionário não encontrado.");

            funcionario.AtualizarNome(dto.Nome);
            await _repository.Atualizar(funcionario);

            // Funcionario "é" um Usuario: os dados de conta (UserName/Email/Ativo)
            // são atualizados através do UsuarioService, que já valida duplicidade
            // de UserName/Email internamente.
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
            try
            {
                await _unidadeTrabalho.IniciarTransacaoAsync();

                var funcionario = await _repository.BuscarPorId(id);

                //ver se está em alguma Os

                //------------------------------
                await _repository.Remover(funcionario!);
                await _usuarioService.Remover(funcionario!.UsuarioId);
                await _unidadeTrabalho.ConfirmarTransacaoAsync();

            }
            catch
            {
                await _unidadeTrabalho.DesfazerTransacaoAsync();
                throw;
            }
           
        }
    }

}
