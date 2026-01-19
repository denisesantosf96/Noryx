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

                    var moedasExternas = await awesomeApi.BuscarMoedasAsync();

                    var moedasDto = moedasExternas.Select(m => new MoedaExternaDto
                    {
                        Codigo = m.Key,
                        Nome = m.Value
                    });

                    await noryxApi.ImportarMoedasAsync(moedasDto);

                    _logger.LogInformation(
                        "Sincronização de moedas concluída. Total recebido: {total}",
                        moedasExternas.Count);

                    var moedas = await noryxApi.BuscarMoedasAsync();  //busca moedas já cadastradas no banco via api noryx

                    const string moedaDestino = "BRL";

                    foreach (var moeda in moedas)
                    {
                        if (moeda.Codigo == moedaDestino)
                            continue;

                        var cotacaoApi = await awesomeApi
                            .BuscarCotacaoAsync(moeda.Codigo, moedaDestino);

                        if (cotacaoApi == null)
                        {
                            _logger.LogWarning(
                                "Cotação não encontrada para {origem}/{destino}",
                                moeda.Codigo, moedaDestino);
                            continue;
                        }

                        var dto = new CotacaoDto
                        {
                            MoedaOrigem = moeda.Codigo,
                            MoedaDestino = moedaDestino,
                            Valor = decimal.Parse(
                                cotacaoApi.bid,
                                CultureInfo.InvariantCulture)
                        };

                        await noryxApi.InserirCotacaoAsync(dto);

                        _logger.LogInformation(
                            "Cotação enviada: {origem}/{destino} → {valor}",
                            dto.MoedaOrigem,
                            dto.MoedaDestino,
                            dto.Valor);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no Worker Noryx.");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }
        }
    }
}
