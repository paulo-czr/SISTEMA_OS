using OS_API.DTOs.Permissao;
using OS_API.DTOs.Tecnico;
using OS_API.DTOs.Usuario;
using OS_API.Models;
using OS_API.Models.Enum;

namespace OS_API.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task AdicionarPermissaoPorTipoUsuario(UsuarioModel usuarioBanco, TipoUsuario tipoUsuario);
        Task<UsuarioModel> Criar(UsuarioModel usuario, string senha);

        Task<UsuarioDto> BuscarPorId(string id);

        Task<List<UsuarioDto>> Listar();

        Task<UsuarioDto> Atualizar(string id, AtualizarUsuarioDto dto);

        Task Remover(string id);

        // Gerenciamento de permissões vinculadas ao usuário (a permissão em si já existe
        // no sistema, cadastrada na tabela Permissao — aqui só vinculamos/desvinculamos).
        Task<List<PermissaoDto>> ListarPermissoes(string id);
        Task<List<PermissaoDto>> AtualizarPermissoes(string id, List<int> idsPermissao);
    }
}
