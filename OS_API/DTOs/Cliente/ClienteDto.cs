using OS_API.Models.Enum;

namespace OS_API.DTOs.Cliente
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string? RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public TipoPessoaEnum TipoPessoa { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public bool Ativo { get; set; }
    }
}