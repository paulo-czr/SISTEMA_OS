using System.Text.Json;
using System.Text.Json.Serialization;

namespace OS_API.Converters
{
    /// <summary>
    /// A API do ViaCEP nem sempre devolve o campo "erro" como um booleano JSON puro
    /// (true/false) — em algumas respostas ele vem como string ("true"/"false").
    /// O conversor padrão do System.Text.Json é estrito e rejeita essa inconsistência,
    /// gerando um erro 500 na aplicação. Este conversor aceita os dois formatos.
    /// </summary>
    public class BooleanFlexivelConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return true;

                case JsonTokenType.False:
                    return false;

                case JsonTokenType.String:
                    // Aceita "true"/"false" (e variações de caixa) vindas como string.
                    return bool.TryParse(reader.GetString(), out var valor) && valor;

                case JsonTokenType.Null:
                    return false;

                default:
                    throw new JsonException(
                        $"Não foi possível converter o valor do tipo '{reader.TokenType}' para booleano.");
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}