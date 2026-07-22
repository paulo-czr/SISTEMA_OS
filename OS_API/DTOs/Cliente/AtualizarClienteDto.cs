using OS_API.Models.Enum;
using OS_API.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OS_API.DTOs.Cliente
{
    /// <summary>
    /// DTO específico para atualização de cliente.
    /// Propositalmente não expõe IdCliente (vem da rota) nem qualquer outro campo de
    /// auditoria/identidade, evitando over-posting: o client não consegue forçar a
    /// alteração de um Id diferente do que está sendo editado.
    /// </summary>
    public class AtualizarClienteDto : IValidatableObject
    {
        [Required(ErrorMessage = "O campo Tipo de Pessoa é obrigatório.")]
        public TipoPessoaEnum TipoPessoa { get; set; }


        [Required(ErrorMessage = "O campo de Nome / Nome Fantasia é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; } = string.Empty;


        // Obrigatório se for Pessoa Jurídica
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


        [StringLength(20, ErrorMessage = "O número deve ter no máximo {1} caracteres.")]
        public string? Numero { get; set; }


        public bool Ativo { get; set; } = true;


        // Roda após todas as validações passarem
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validação cruzada para Pessoa Jurídica
            if (TipoPessoa == TipoPessoaEnum.Juridica && string.IsNullOrWhiteSpace(RazaoSocial))
            {
                yield return new ValidationResult(
                    "A Razão Social é obrigatória para Pessoa Jurídica.",
                    new[] { nameof(RazaoSocial) });
            }

            // Validação cruzada para Pessoa Física
            if (TipoPessoa == TipoPessoaEnum.Fisica && !string.IsNullOrWhiteSpace(RazaoSocial))
            {
                yield return new ValidationResult(
                    "Pessoa Física não deve possuir Razão Social.",
                    new[] { nameof(RazaoSocial) });
            }
        }
    }
}
