using OS_API.DTOs.ViaCepDto;
using OS_API.Interfaces.Services;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace OS_API.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        private const string ViaCepBaseUrl = "https://viacep.com.br/ws";

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepDto?> ObterEnderecoPorCepAsync(string cep)
        {
            // Remove pontos, traços e caracteres não numéricos
            var cepLimpo = Regex.Replace(cep ?? "", @"[^\d]", "");

            if (cepLimpo.Length != 8) return null;

            string url = $"{ViaCepBaseUrl}/{cepLimpo}/json/";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var resultado = await response.Content.ReadFromJsonAsync<ViaCepDto>();

            if (resultado == null || resultado.Erro) return null;

            return resultado;
        }
    }
}