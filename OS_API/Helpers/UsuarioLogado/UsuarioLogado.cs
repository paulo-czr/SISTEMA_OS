using OS_API.Exceptionn;
using OS_API.Interfaces.Services;
using System.Security.Claims;

namespace OS_API.Helpers.UsuarioLogado
{
    public class UsuarioLogado : IUsuarioLogado
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioService _usuarioService;

        public UsuarioLogado(IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuarioService = usuarioService;
        }

        private ClaimsPrincipal? Usuario =>
            _httpContextAccessor.HttpContext?.User;

        public bool Autenticado =>
            Usuario?.Identity?.IsAuthenticated ?? false;

        public string? IdUsuario =>
            Usuario?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? UserName =>
            Usuario?.FindFirst(ClaimTypes.Name)?.Value;

        public string? Email =>
            Usuario?.FindFirst(ClaimTypes.Email)?.Value;

        public IEnumerable<Claim> Claims =>
            Usuario?.Claims ?? Enumerable.Empty<Claim>();

        public string retornarUserLogado()
        {
            //pegar o usuario que registrou
            if (!Autenticado)
                throw new UnauthorizedAccessException();
            return IdUsuario!;
        }

        public async Task<int> RetornarIdFuncionarioLogado()
        {
            var usuario = await _usuarioService.BuscarPorId(retornarUserLogado());

            if (usuario == null)
                throw new EntidadeNaoEncontradaException("Usuário não encontrado.");

            if (usuario.IdFuncionario == null)
                throw new Exception("O usuário logado não possui um funcionário vinculado.");

            return usuario.IdFuncionario.Value;
        }


    }
}
