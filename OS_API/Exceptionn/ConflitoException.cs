namespace OS_API.Exceptionn
{
    public class ConflitoException : Exception
    {
        public ConflitoException(string mensagem)
            : base(mensagem)
        {
            //throw new ConflitoException("Já existe um técnico com esse CPF.");
        }
    }
}
