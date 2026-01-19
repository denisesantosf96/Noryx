using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Noryx.API.Application.Dtos;
using RestSharp;

namespace WorkerIntegracao.Services
    {
        public class ApiNoryx : IApiNoryx
        {
            private readonly string baseUrl = "https://localhost:7211/api/";

            public async Task<IEnumerable<MoedaExternaDto>> BuscarMoedasAsync()
            {
                var client = new RestClient($"{baseUrl}moeda");
                var request = new RestRequest("", Method.Get);

                var response = await client
                    .ExecuteAsync<IEnumerable<MoedaExternaDto>>(request);

                if (!response.IsSuccessful || response.Data == null)
                    return Enumerable.Empty<MoedaExternaDto>();

                return response.Data; 
            }

        public async Task ImportarMoedasAsync(IEnumerable<MoedaExternaDto> moedas)
        {
            var client = new RestClient($"{baseUrl}moeda");
            var request = new RestRequest("", Method.Post);

            request.AddJsonBody(moedas);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception("Erro ao importar moedas na API Noryx");
        }

        public async Task InserirCotacaoAsync(CotacaoDto dto)
            {
                var client = new RestClient($"{baseUrl}Cotacao");
                var request = new RestRequest("", Method.Post);

                request.AddJsonBody(dto);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                    throw new Exception("Erro ao enviar cotação para API Noryx");
            }
        }
}
