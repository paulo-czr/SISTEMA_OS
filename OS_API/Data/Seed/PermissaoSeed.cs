using Microsoft.EntityFrameworkCore;
using OS_API.Helpers.Constantes;
using OS_API.Models;

namespace OS_API.Data.Seed
{
    public static class PermissaoSeed
    {
        public static async Task SeedPermissoesAsync(AppDbContext context)
        {
            var permissoes = new List<PermissaoModel>
            {
                // Funcionário
                new PermissaoModel
                {
                    Nome = Permissoes.FuncionarioVisualizar,
                    Descricao = "Permite visualizar funcionários.",
                    Modulo = "Funcionario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.FuncionarioCriar,
                    Descricao = "Permite cadastrar funcionários.",
                    Modulo = "Funcionario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.FuncionarioAtualizar,
                    Descricao = "Permite atualizar funcionários.",
                    Modulo = "Funcionario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.FuncionarioExcluir,
                    Descricao = "Permite excluir funcionários.",
                    Modulo = "Funcionario"
                },


                // Cliente
                new PermissaoModel
                {
                    Nome = Permissoes.ClienteVisualizar,
                    Descricao = "Permite visualizar clientes.",
                    Modulo = "Cliente"
                },

                new PermissaoModel
                {
                    Nome = Permissoes.ClienteCriar,
                    Descricao = "Permite cadastrar clientes.",
                    Modulo = "Cliente"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.ClienteAtualizar,
                    Descricao = "Permite atualizar clientes.",
                    Modulo = "Cliente"
                },

                new PermissaoModel
                {
                    Nome = Permissoes.ClienteExcluir,
                    Descricao = "Permite excluir clientes.",
                    Modulo = "Cliente"
                },


                // Ordem de Serviço
                new PermissaoModel
                {
                    Nome = Permissoes.OSVisualizarTodas,
                    Descricao = "Permite que o usuario veja todas as ordens de serviço",
                    Modulo = "OS"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.OSCriar,
                    Descricao = "Permite cadastrar ordens de serviço.",
                    Modulo = "OS"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.OSAtualizar,
                    Descricao = "Permite atualizar ordens de serviço.",
                    Modulo = "OS"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.OSExcluir,
                    Descricao = "Permite excluir ordens de serviço.",
                    Modulo = "OS"
                },


                // Usuário
                new PermissaoModel
                {
                    Nome = Permissoes.UsuarioVisualizar,
                    Descricao = "Permite visualizar usuários.",
                    Modulo = "Usuario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.UsuarioAtualizar,
                    Descricao = "Permite atualizar usuários.",
                    Modulo = "Usuario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.UsuarioRemover,
                    Descricao = "Permite remover usuários.",
                    Modulo = "Usuario"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.UsuarioGerenciarPermissoes,
                    Descricao = "Permite gerenciar permissões dos usuários.",
                    Modulo = "Usuario"
                },


                // Tipo de Atendimento
                new PermissaoModel
                {
                    Nome = Permissoes.TipoAtendimentoVisualizar,
                    Descricao = "Permite visualizar tipos de atendimento.",
                    Modulo = "TipoAtendimento"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.TipoAtendimentoCriar,
                    Descricao = "Permite cadastrar tipos de atendimento.",
                    Modulo = "TipoAtendimento"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.TipoAtendimentoAtualizar,
                    Descricao = "Permite atualizar tipos de atendimento.",
                    Modulo = "TipoAtendimento"
                },
                new PermissaoModel
                {
                    Nome = Permissoes.TipoAtendimentoExcluir,
                    Descricao = "Permite excluir tipos de atendimento.",
                    Modulo = "TipoAtendimento"
                }
            };  

            var nomesExistentes = await context.Permissoes
                .Select(p => p.Nome)
                .ToListAsync();

            var permissoesNovas = permissoes
                .Where(p => !nomesExistentes.Contains(p.Nome))
                .ToList();

            if (permissoesNovas.Any())
            {
                await context.Permissoes.AddRangeAsync(permissoesNovas);
                await context.SaveChangesAsync();
            }
        }
    }
}