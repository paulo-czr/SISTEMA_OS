using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OS_API.DTOs.Tecnico;
using OS_API.Helpers.Constantes;
using OS_API.Interfaces.Repositories;
using OS_API.Models;
using OS_API.Models.Enum;
using System.Security.Claims;

namespace OS_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<UsuarioModel> _userManager;

        public UsuarioRepository(UserManager<UsuarioModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UsuarioModel?> BuscarPeloEmail(string usuario)
        {
            return await _userManager.FindByEmailAsync(usuario);
        }

        public async Task<UsuarioModel?> BuscarPeloUserName(string usuario)
        {
            return await _userManager.FindByNameAsync(usuario);
        }

        public async Task<UsuarioModel?> BuscarPeloUserEmail(string usuario)
        {
            UsuarioModel? u;
            if (usuario.Contains("@"))
            {
                u = await BuscarPeloEmail(usuario);
            }
            else
            {
                u = await BuscarPeloUserName(usuario);
            }
            return u;
        }

        public async Task<UsuarioModel?> BuscarPorId(string id)
        {
            // FindByIdAsync não permite Include, então consultamos direto em Users
            // (IQueryable normal do EF) pra trazer o Funcionario vinculado junto.
            return await _userManager.Users
                .Include(u => u.Funcionario)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<UsuarioModel>> Listar()
        {
            // _userManager.Users é um IQueryable direto sobre a tabela do Identity.
            return await _userManager.Users
                .Include(u => u.Funcionario)
                .ToListAsync();
        }

        public async Task Atualizar(UsuarioModel usuario)
        {
            // SetUserNameAsync/SetEmailAsync atualizam também os campos "Normalized*"
            // usados nas buscas (FindByNameAsync/FindByEmailAsync). Alterar UserName/Email
            // direto na entidade e só chamar UpdateAsync deixaria esses campos desatualizados.
            await _userManager.SetUserNameAsync(usuario, usuario.UserName);
            await _userManager.SetEmailAsync(usuario, usuario.Email);

            // Persiste os demais campos (ex.: Ativo).
            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
                throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));
        }

        public async Task Remover(UsuarioModel usuario)
        {
            var resultado = await _userManager.DeleteAsync(usuario);

            if (!resultado.Succeeded)
                throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));
        }

        public async Task<bool> ValidarSenha(UsuarioModel usuario, string senha)
        {
            // Confere a senha usando o Identity
            var senhaValida = await _userManager.CheckPasswordAsync(usuario, senha);
            return senhaValida;
        }

        public async Task<IList<Claim>> BuscarClaims(UsuarioModel usuario)
        {
            return await _userManager.GetClaimsAsync(usuario);
        }

        public async Task<UsuarioModel> Criar(UsuarioModel usuario, string senha)
        {
            var resultado = await _userManager.CreateAsync(usuario, senha);
            if (!resultado.Succeeded)
            {
                throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }
            return usuario;
        }

        public async Task AdicionarPermissaoPorTipoUser(UsuarioModel usuario, TipoUsuario tipo)
        {
            await _userManager.AddToRoleAsync(usuario, tipo.ToString());

            var permissoes = MapeamentoPermissoes.ObterPermissoes(tipo);

            foreach (var permissao in permissoes)
            {
                await _userManager.AddClaimAsync(usuario, new Claim("Permissao", permissao));
            }
        }

        public async Task<List<string>> BuscarPermissoes(UsuarioModel usuario)
        {
            var claims = await _userManager.GetClaimsAsync(usuario);

            return claims
                .Where(c => c.Type == "Permissao")
                .Select(c => c.Value)
                .ToList();
        }

        public async Task SincronizarPermissoes(UsuarioModel usuario, List<string> nomesPermissoes)
        {
            var claimsAtuais = await _userManager.GetClaimsAsync(usuario);
            var claimsDePermissao = claimsAtuais.Where(c => c.Type == "Permissao").ToList();

            // Remove todas as permissões atuais do usuário antes de inserir a nova lista,
            // assim não corre o risco de ficar claim duplicada para a mesma permissão.
            if (claimsDePermissao.Any())
                await _userManager.RemoveClaimsAsync(usuario, claimsDePermissao);

            var novasClaims = nomesPermissoes
                .Select(nome => new Claim("Permissao", nome))
                .ToList();

            if (novasClaims.Any())
                await _userManager.AddClaimsAsync(usuario, novasClaims);
        }
    }
}