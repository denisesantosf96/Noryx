using System;
using System.Collections.Generic;
using System.Linq;
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

            var response = await client.ExecuteAsync<IEnumerable<MoedaExternaDto>>(request);

            if (!response.IsSuccessful || response.Data == null)
                return Enumerable.Empty<MoedaExternaDto>();

            return response.Data;
        }

        public async Task ImportarMoedasAsync(IEnumerable<MoedaExternaDto> moedas)
        {
            var client = new RestClient($"{baseUrl}moeda/importar");
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(moedas);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var erro = response.Content ?? "Sem resposta da API";
                throw new Exception($"Erro ao importar moedas na API Noryx: {erro}");
            }
        }

        public async Task InserirCotacaoAsync(CotacaoDto dto)
        {
            var client = new RestClient($"{baseUrl}cotacao");
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(dto);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var erro = response.Content ?? "Sem resposta da API";
                throw new Exception($"Erro ao enviar cotação para API Noryx: {erro}");
            }
        }
    }
}
