namespace OS_API.Validation.Helpers
{
    /// <summary>
    /// Utilitário responsável por extrair apenas os caracteres numéricos de uma string.
    /// Usado tanto pelos validadores (Cpf, Cnpj, Cep) quanto pela camada de serviço,
    /// para garantir que documentos e CEPs sejam sempre validados e persistidos
    /// de forma normalizada (sem pontos, traços ou barras).
    /// </summary>
    public static class SomenteDigitos
    {
        public static string Extrair(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return new string(valor.Where(char.IsDigit).ToArray());
        }
    }
}
