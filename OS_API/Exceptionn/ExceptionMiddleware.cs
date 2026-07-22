using OS_API.Exceptionn.OS_API.Exceptions;
using System.Text.Json;

namespace OS_API.Exceptionn
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratarExcecao(context, ex);
            }
        }

        private static async Task TratarExcecao(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case EntidadeNaoEncontradaException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case ValidacaoException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case ConflitoException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                case NaoAutorizadoException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case ProibidoException:
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var resposta = new
            {
                Status = context.Response.StatusCode,
                Mensagem = ex.Message
            };

            var json = JsonSerializer.Serialize(resposta);

            await context.Response.WriteAsync(json);
        }
    }
}
