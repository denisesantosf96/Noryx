using System.Globalization;
using Noryx.API.Application.Dtos;
using Noryx.API.Application.Services;
using WorkerIntegracao.Services;

namespace WorkerIntegracao
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker Noryx iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var awesomeApi = scope.ServiceProvider
                        .GetRequiredService<IApiAwesome>();

                    var noryxApi = scope.ServiceProvider
                        .GetRequiredService<IApiNoryx>();

                    var moedaOrigem = "USD";
                    var moedaDestino = "BRL";

                    var cotacaoApi = await awesomeApi
                        .BuscarCotacaoAsync(moedaOrigem, moedaDestino);

                    if (cotacaoApi == null)
                    {
                        _logger.LogWarning("Cotação não retornada.");
                        continue;
                    }

                    var dto = new CotacaoDto
                    {
                        MoedaOrigem = moedaOrigem,
                        MoedaDestino = moedaDestino,
                        Valor = decimal.Parse(
                            cotacaoApi.bid,
                            CultureInfo.InvariantCulture)
                    };

                    await noryxApi.InserirCotacaoAsync(dto);

                    _logger.LogInformation(
                        "Cotação enviada para API {origem}/{destino} → {valor}",
                        moedaOrigem, moedaDestino, dto.Valor);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no Worker Noryx.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
