using OS_API.Models.Enum;
using OS_API.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Cliente
{

    public class CriarClienteDto : IValidatableObject
    {
        [Required(ErrorMessage = "O campo Tipo de Pessoa é obrigatório.")]
        public TipoPessoaEnum TipoPessoa { get; set; }

        [Required(ErrorMessage = "O campo de Nome / Nome Fantasia é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; } = string.Empty;

        // Opcional no banco, mas obrigatório no negócio se for Pessoa Jurídica
        [StringLength(100, ErrorMessage = "A Razão Social deve ter no máximo 100 caracteres.")]
        public string? RazaoSocial { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório.")]
        [DocumentoValido<TipoPessoaEnum>(nameof(TipoPessoa))]
        public string Documento { get; set; } = string.Empty;

        [EmailValido]
        [StringLength(254)]
        public string? Email { get; set; }

        [TelefoneValido]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [Cep]
        public string Cep { get; set; } = string.Empty;

        [StringLength(2, ErrorMessage = "O campo UF deve ter no máximo {1} caracteres.")]
        public string? Uf { get; set; }

        [StringLength(150, ErrorMessage = "O campo Cidade deve ter no máximo {1} caracteres.")]
        public string? Cidade { get; set; }

        [StringLength(150, ErrorMessage = "O campo Bairro deve ter no máximo {1} caracteres.")]
        public string? Bairro { get; set; }

        [StringLength(150, ErrorMessage = "O campo Rua deve ter no máximo {1} caracteres.")]
        public string? Rua { get; set; }

        [StringLength(200, ErrorMessage = "O campo Complemento deve ter no máximo {1} caracteres.")]
        public string? Complemento { get; set; }

        [StringLength(20, ErrorMessage = "O número deve ter no máximo {1} caracteres.")]
        public string? Numero { get; set; }




        // Esse método roda automaticamente se os atributos acima passarem
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validação cruzada para Pessoa Jurídica
            if (TipoPessoa == TipoPessoaEnum.Juridica)
            {
                if (string.IsNullOrWhiteSpace(RazaoSocial))
                {
                    yield return new ValidationResult(
                        "A Razão Social é obrigatória para Pessoa Jurídica.",
                        new[] { nameof(RazaoSocial) });
                }
            }

            // Validação cruzada para Pessoa Física
            if (TipoPessoa == TipoPessoaEnum.Fisica)
            {
                if (!string.IsNullOrWhiteSpace(RazaoSocial))
                {
                    yield return new ValidationResult(
                        "Pessoa Física não deve possuir Razão Social.",
                        new[] { nameof(RazaoSocial) });
                }
            }
        }
    }
}