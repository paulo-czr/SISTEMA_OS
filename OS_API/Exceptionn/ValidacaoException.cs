namespace OS_API.Exceptionn
{
    public class ValidacaoException : Exception
    {
        public ValidacaoException(string mensagem)
            : base(mensagem)
        {
            //exemplo
            //throw new ValidacaoException("O CPF é obrigatório.");
            //throw new ValidacaoException("O e-mail informado é inválido.");
        }
    }
}
