using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OS_API.Validation.Attributes
{
    public class TelefoneValidoAttribute : ValidationAttribute
    {
        // Aceita fixo (10 dígitos) ou celular (11 dígitos), com DDD de 11 a 99.
        private static readonly Regex _regex = new(@"^[1-9]{2}9?\d{8}$");

        public TelefoneValidoAttribute()
            : base("O telefone informado é inválido.") { }

        public override bool IsValid(object? value)
        {
            var texto = value?.ToString();

            if (string.IsNullOrWhiteSpace(texto))
                return true;

            var digitos = new string(texto.Where(char.IsDigit).ToArray());
            return _regex.IsMatch(digitos);
        }
    }
}