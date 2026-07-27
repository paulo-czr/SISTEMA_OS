namespace OS_API.DTOs.Assinatura
{
    // Enviado pela página pública quando o cliente termina de assinar.
    public class SubmeterAssinaturaClienteDto
    {
        public string NomeSignatario { get; set; } = string.Empty;
        public string? DocumentoSignatario { get; set; }
        public string ImagemAssinatura { get; set; } = string.Empty; // base64 (PNG)

        // PDF final (relatório + as 2 assinaturas), montado no FRONT com pdf-lib.
        // É um byte[] de propósito: o System.Text.Json já serializa/desserializa
        // byte[] como base64 automaticamente, então o front só manda uma string
        // e aqui já chega pronto como bytes — sem precisar mudar o tipo da coluna.
        public byte[] ArquivoPdf { get; set; } = Array.Empty<byte>();
    }
}
