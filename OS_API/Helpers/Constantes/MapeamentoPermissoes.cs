using OS_API.Models.Enum;

namespace OS_API.Helpers.Constantes
{
    public static class MapeamentoPermissoes
    {
        //esse metodo mapea as pemissao para ser chamado no repository e gravar as permissao por tipo de user
        public static List<string> ObterPermissoes(TipoUsuario tipo)
        {
            return tipo switch
            {
            TipoUsuario.Administrador => new()
            {
                Permissoes.FuncionarioVisualizar,
                Permissoes.FuncionarioCriar,
                Permissoes.FuncionarioAtualizar,
                Permissoes.FuncionarioExcluir,
                // Cliente
                Permissoes.ClienteVisualizar,
                Permissoes.ClienteCriar,
                Permissoes.ClienteAtualizar,
                Permissoes.ClienteExcluir,
                // Ordem de Serviço
                Permissoes.OSVisualizarTodas,
                Permissoes.OSCriar,
                Permissoes.OSAtualizar,
                Permissoes.OSExcluir,
                // Usuário
                Permissoes.UsuarioVisualizar,
                Permissoes.UsuarioAtualizar,
                Permissoes.UsuarioRemover,
                Permissoes.UsuarioGerenciarPermissoes,
                Permissoes.UsuarioVisualizar,
                // Tipo de Atendimento
                Permissoes.TipoAtendimentoVisualizar,
                Permissoes.TipoAtendimentoCriar,
                Permissoes.TipoAtendimentoAtualizar,
                Permissoes.TipoAtendimentoExcluir
            },


            TipoUsuario.Gestor => new()
            {
                Permissoes.FuncionarioVisualizar,
                Permissoes.FuncionarioCriar,
                Permissoes.FuncionarioAtualizar,
                Permissoes.FuncionarioExcluir,
                // Cliente
                Permissoes.ClienteVisualizar,
                Permissoes.ClienteCriar,
                Permissoes.ClienteAtualizar,
                Permissoes.ClienteExcluir,
                // Ordem de Serviço
                Permissoes.OSVisualizarTodas,
                Permissoes.OSCriar,
                Permissoes.OSAtualizar,
                Permissoes.OSExcluir,
                // Usuário
                Permissoes.UsuarioVisualizar,
                Permissoes.UsuarioAtualizar,
                Permissoes.UsuarioRemover,
                Permissoes.UsuarioGerenciarPermissoes,
                Permissoes.UsuarioVisualizar,
                // Tipo de Atendimento
                Permissoes.TipoAtendimentoVisualizar,
                Permissoes.TipoAtendimentoCriar,
                Permissoes.TipoAtendimentoAtualizar,
                Permissoes.TipoAtendimentoExcluir
            },

            TipoUsuario.Tecnico => new()
            {
                Permissoes.FuncionarioVisualizar,

                // Cliente
                Permissoes.ClienteVisualizar,
                Permissoes.ClienteCriar,
                Permissoes.ClienteAtualizar,

                // OS
                Permissoes.OSCriar,
                Permissoes.OSAtualizar,

                // Tipo de Atendimento
                Permissoes.TipoAtendimentoVisualizar,
                Permissoes.TipoAtendimentoCriar,
                Permissoes.TipoAtendimentoAtualizar
            },
                _ => new()
            };
        }
    }
}