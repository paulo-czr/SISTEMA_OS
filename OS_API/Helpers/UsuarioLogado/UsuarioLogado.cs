using System.Security.Claims;

namespace OS_API.Helpers.UsuarioLogado
{
    public class UsuarioLogado : IUsuarioLogado
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioLogado(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
    }
}
