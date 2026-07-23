namespace OS_API.DTOs.Usuario
{
    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public int? IdFuncionario { get; set; }

        public string? NomeFuncionario { get; set; }
    }
}
