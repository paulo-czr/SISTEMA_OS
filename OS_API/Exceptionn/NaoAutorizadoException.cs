namespace OS_API.Exceptionn
{
    public class NaoAutorizadoException : Exception
    {
        public NaoAutorizadoException(string mensagem)
            : base(mensagem)
        {
            //throw new NaoAutorizadoException("Usuário não autenticado.");
        }
    }
}
