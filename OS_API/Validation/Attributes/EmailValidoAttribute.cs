using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace OS_API.Validation.Attributes
{
    /// <summary>
    /// Valida o formato de um e-mail combinando análise por Regex de alta performance 
    /// e a validação oficial do .NET (MailAddress).
    /// </summary>
    /// 
    /// <remarks>
    /// Campos nulos ou vazios são considerados válidos por este atributo. 
    /// A obrigatoriedade do preenchimento deve ser tratada pelo atributo [Required] no dto.
    /// </remarks>
    /// 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmailValidoAttribute : ValidationAttribute
    {
        private const int TamanhoMaximo = 254;

        // Validação inicial rápida: exige estrutura estrutural básica sem espaços.
        // O uso de Timeout protege a aplicação contra ataques de negação de serviço (ReDoS).
        private static readonly Regex FormatoEstruturalEmail = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(100));

        public EmailValidoAttribute() : base("O e-mail informado é inválido.")
        {
        }

        public override bool IsValid(object? value)
        {
            // Validação de ausência de dados fica para o [Required]
            if (value is not string email || string.IsNullOrWhiteSpace(email))
                return true;

            if (email.Length > TamanhoMaximo)
                return false;

            if (!FormatoEstruturalEmail.IsMatch(email))
                return false;

            // Se o formato violar regras complexas de domínio ou caracteres, 
            // o construtor disparará uma FormatException.
            try
            {
                // O uso do operador de descarte (_) matém o foco somente em validar o email
                _ = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}