using OS_API.Validation.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OS_API.Validation.Attributes
{
    /// <summary>
    /// Valida se o valor informado é um CPF válido (formato + dígitos verificadores).
    /// Campos nulos ou vazios são considerados válidos por este atributo:
    /// a obrigatoriedade deve ser tratada separadamente com [Required].
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CpfAttribute : ValidationAttribute
    {
        public CpfAttribute() : base("O CPF informado é inválido.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is not string cpf || string.IsNullOrWhiteSpace(cpf))
                return true;

            return CpfValidator.EhValido(cpf);
        }
    }
}
