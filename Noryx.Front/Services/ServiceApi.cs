using System.Text.Json;
using Noryx.Front.ViewModels;

namespace Noryx.Front.Services
{
    public class ServiceApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ServiceApi(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]);
        }

        public async Task<CotacaoViewModel> GetCotacao(string origem, string destino)
        {
            var response = await _httpClient.GetAsync($"cotacao/{origem}/{destino}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CotacaoViewModel>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
