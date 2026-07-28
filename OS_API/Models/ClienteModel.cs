using OS_API.Models.Enum;

namespace OS_API.Models.Cliente
{
    public class ClienteModel
    {
        public ClienteModel() { }

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
        public string? Uf { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Rua { get; set; }
        public string? Complemento { get; set; }
        public string? Numero { get; set; }
        
        public bool Ativo { get; set; }


        /// <summary>
        /// Pessoa Física nunca deve manter Razão Social preenchida — só Pessoa Jurídica.
        /// </summary>
        private static string? NormalizarRazaoSocial(TipoPessoaEnum tipoPessoa, string? razaoSocial)
        {
            if (tipoPessoa == TipoPessoaEnum.Fisica)
                return null;

            return string.IsNullOrWhiteSpace(razaoSocial) ? null : razaoSocial.Trim();
        }


        // Método usado para atualizar cliente
        public void AtualizarDados(
            TipoPessoaEnum tipoPessoa,
            string nomeFantasia,
            string? razaoSocial,
            string documentoNormalizado,
            string? telefone,
            string? emailNormalizado,
            string cepNormalizado,
            string? rua,
            string? cidade,
            string? uf,
            string? bairro,
            string? complemento,
            string? numero,
            bool ativo)
        {
            TipoPessoa = tipoPessoa;
            NomeFantasia = nomeFantasia.Trim();
            RazaoSocial = NormalizarRazaoSocial(tipoPessoa, razaoSocial);
            Documento = documentoNormalizado;
            Telefone = telefone;
            Email = emailNormalizado;
            Cep = cepNormalizado;
            Rua = rua;
            Cidade = cidade;
            Uf = uf;
            Bairro = bairro;
            Complemento = complemento;
            Numero = numero;
            Ativo = ativo;
        }

    }
}