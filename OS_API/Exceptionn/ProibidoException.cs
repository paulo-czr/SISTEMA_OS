namespace OS_API.Exceptionn
{
    namespace OS_API.Exceptions
    {
        public class ProibidoException : Exception
        {
            public ProibidoException(string mensagem)
                : base(mensagem)
            {
                //throw new ProibidoException("Você não possui permissão para excluir este registro.");
            }
        }
    }
}
