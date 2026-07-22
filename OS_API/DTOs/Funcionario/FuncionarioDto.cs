namespace OS_API.DTOs.Tecnico
{
    public class FuncionarioDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string UsuarioId { get; set; }

        // Dados vindos do Usuario vinculado (Funcionario "é" um Usuario, então
        // faz sentido devolver esses dados junto ao consultar o Funcionario).
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}
