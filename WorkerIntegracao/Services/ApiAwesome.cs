using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<Dictionary<string, string>> BuscarMoedasAsync()
        {
            var client = new RestClient($"{baseUrl}available");
            var request = new RestRequest("", Method.Get);

            var response = await client
                .ExecuteAsync<Dictionary<string, string>>(request);

            if (!response.IsSuccessful || response.Data == null)
                return new Dictionary<string, string>();

            return response.Data;
        }

    }
}
