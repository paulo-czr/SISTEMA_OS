namespace OS_API.DTOs.Assinatura
{
    // Devolvido depois de iniciar a assinatura — o front monta o link
    // (ex.: https://seusistema.com/assinar/{Token}) e o QR code a partir disso.
    public class TokenAssinaturaDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
    }
}
