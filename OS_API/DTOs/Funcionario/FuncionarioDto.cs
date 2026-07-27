namespace OS_API.DTOs.Tecnico
{
    public class FuncionarioDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string UsuarioId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public string? AssinaturaPadrao { get; set; }
    }
}
