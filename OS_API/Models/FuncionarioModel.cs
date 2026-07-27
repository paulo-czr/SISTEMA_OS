namespace OS_API.Models
{
    public class FuncionarioModel
    {
        public int Id { get; private set; }

        public string Nome { get; private set; }

        public string UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }

        // Assinatura desenhada uma vez e reaproveitada nos próximos relatórios,
        // sem precisar assinar de novo toda hora (base64, PNG).
        public string? AssinaturaPadrao { get; private set; }

        protected FuncionarioModel() { }

        public FuncionarioModel(
            string nome,
            string usuarioId)
        {
            Nome = nome;
            UsuarioId = usuarioId;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

        public void AtualizarAssinaturaPadrao(string imagemAssinatura)
        {
            AssinaturaPadrao = imagemAssinatura;
        }
    }
}

