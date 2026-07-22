namespace OS_API.Models
{
    public class FuncionarioModel
    {
        public int Id { get; private set; }

        public string Nome { get; private set; }

        public string UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }


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
    }
}
