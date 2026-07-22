using OS_API.Models.Enum;

namespace OS_API.Models.Cliente
{
    public class ClienteModel
    {
        public int IdCliente { get; set; }
        
        public TipoPessoaEnum TipoPessoa { get; set; }

        // Deve ser obrigatório (não-nulo), pois guarda o Nome (PF) ou Fantasia (PJ)
        public string NomeFantasia { get; set; } = string.Empty;

        // Deve aceitar NULO (string?), pois clientes PF não terão esse dado
        public string? RazaoSocial { get; set; }

        public string Documento { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        public string Cep { get; set; } = string.Empty;
        public string? Rua { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Numero { get; set; }
        
        public bool Ativo { get; set; }

    }
}