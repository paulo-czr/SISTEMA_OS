using System.Text.Json.Serialization;
using OS_API.Converters;

namespace OS_API.DTOs.ViaCepDto
{
    public class ViaCepDto
    {
        [JsonPropertyName("cep")]
        public string? Cep {  get; set; }

        [JsonPropertyName("uf")]
        public string? Uf { get; set; }

        [JsonPropertyName("localidade")]
        public string? Cidade { get; set; }

        [JsonPropertyName("logradouro")]
        public string? Rua {  get; set; }

        [JsonPropertyName("complemento")]
        public string? Complemento { get; set; }

        [JsonPropertyName("bairro")]
        public string? Bairro { get; set; }

        [JsonPropertyName("erro")]
        [JsonConverter(typeof(BooleanFlexivelConverter))]
        public bool Erro { get; set; }
    }
}
