namespace OS_API.DTOs.Permissao
{
    public class PermissaoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Modulo { get; set; } = string.Empty;
    }
}
