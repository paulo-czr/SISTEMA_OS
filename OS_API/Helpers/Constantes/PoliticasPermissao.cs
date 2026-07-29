using Microsoft.AspNetCore.Authorization;

namespace OS_API.Helpers.Constantes
{
    public static class PoliticasPermissao
    {
        // esse metodo e para ser chamado na program 
        //como vai ser muita permissao criei ele aqui
        public static void AdicionarPoliticas(AuthorizationOptions options)
        {

            // Funcionario
            options.AddPolicy(Permissoes.FuncionarioVisualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.FuncionarioVisualizar));

            options.AddPolicy(Permissoes.FuncionarioCriar,
                policy => policy.RequireClaim("Permissao", Permissoes.FuncionarioCriar));

            options.AddPolicy(Permissoes.FuncionarioAtualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.FuncionarioAtualizar));

            options.AddPolicy(Permissoes.FuncionarioExcluir,
                policy => policy.RequireClaim("Permissao", Permissoes.FuncionarioExcluir));


            // Cliente
            options.AddPolicy(Permissoes.ClienteVisualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.ClienteVisualizar));

            options.AddPolicy(Permissoes.ClienteCriar,
                policy => policy.RequireClaim("Permissao", Permissoes.ClienteCriar));

            options.AddPolicy(Permissoes.ClienteAtualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.ClienteAtualizar));

            options.AddPolicy(Permissoes.ClienteExcluir,
                policy => policy.RequireClaim("Permissao", Permissoes.ClienteExcluir));


            // OS
            options.AddPolicy(Permissoes.OSVisualizarTodas,
                policy => policy.RequireClaim("Permissao", Permissoes.OSVisualizarTodas));

            options.AddPolicy(Permissoes.OSCriar,
                policy => policy.RequireClaim("Permissao", Permissoes.OSCriar));

            options.AddPolicy(Permissoes.OSAtualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.OSAtualizar));

            options.AddPolicy(Permissoes.OSExcluir,
                policy => policy.RequireClaim("Permissao", Permissoes.OSExcluir));


            // Usuario
            options.AddPolicy(Permissoes.UsuarioVisualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.UsuarioVisualizar));

            options.AddPolicy(Permissoes.UsuarioAtualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.UsuarioAtualizar));

            options.AddPolicy(Permissoes.UsuarioRemover,
                policy => policy.RequireClaim("Permissao", Permissoes.UsuarioRemover));

            options.AddPolicy(Permissoes.UsuarioGerenciarPermissoes,
                policy => policy.RequireClaim("Permissao", Permissoes.UsuarioGerenciarPermissoes));


            // Tipo Atendimento
            options.AddPolicy(Permissoes.TipoAtendimentoVisualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.TipoAtendimentoVisualizar));

            options.AddPolicy(Permissoes.TipoAtendimentoCriar,
                policy => policy.RequireClaim("Permissao", Permissoes.TipoAtendimentoCriar));

            options.AddPolicy(Permissoes.TipoAtendimentoAtualizar,
                policy => policy.RequireClaim("Permissao", Permissoes.TipoAtendimentoAtualizar));

            options.AddPolicy(Permissoes.TipoAtendimentoExcluir,
                policy => policy.RequireClaim("Permissao", Permissoes.TipoAtendimentoExcluir));

        }
    }
}
