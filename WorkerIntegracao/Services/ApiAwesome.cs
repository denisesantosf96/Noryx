using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using WorkerIntegracao.Models.Awesome;

namespace WorkerIntegracao.Services
{
    public class ApiAwesome : IApiAwesome
    {
        private readonly string baseUrl = "https://economia.awesomeapi.com.br/json/";

        public async Task<CotacaoAwesome> BuscarCotacaoAsync(string moedaOrigem, string moedaDestino)
        {
            var par = $"{moedaOrigem}-{moedaDestino}";

            var client = new RestClient($"{baseUrl}last/{par}");
            var request = new RestRequest("", Method.Get);

            var response = await client.ExecuteAsync<Dictionary<string, CotacaoAwesome>>(request);

            if (!response.IsSuccessful || response.Data == null)
                return null;

            var chave = $"{moedaOrigem}{moedaDestino}";
            return response.Data[chave];
        }
    }
}
