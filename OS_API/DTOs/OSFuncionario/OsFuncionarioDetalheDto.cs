namespace OS_API.DTOs.OSFuncionario
{
    public class OsFuncionarioDetalheDto
    {
        public int IdOsFuncionario { get; set; }

        public int IdFuncionario { get; set; }

        public string NomeFuncionario { get; set; } = string.Empty;

        public bool Responsavel { get; set; }
    }
}
