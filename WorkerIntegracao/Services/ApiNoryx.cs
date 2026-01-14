using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noryx.API.Application.Dtos;
using RestSharp;

namespace WorkerIntegracao.Services
{
    public class ApiNoryx : IApiNoryx
    {
        private readonly string baseUrl = "https://localhost:7211/api/";

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
