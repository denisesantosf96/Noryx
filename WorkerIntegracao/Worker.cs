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

                    _logger.LogInformation("Iniciando sincronização de moedas...");

                    var moedasExistentes = await noryxApi.BuscarMoedasAsync();
                    var codigosExistentes = moedasExistentes
                        .Select(m => m.Codigo)
                        .ToHashSet();

                    var moedasExternas = await awesomeApi.BuscarMoedasAsync();

                    _logger.LogInformation(
                        "Total de pares recebidos da AwesomeAPI: {total}",
                        moedasExternas.Count);

                    var codigosExternosUnicos = moedasExternas
                        .Select(m => m.Key.Split('-')[0])
                        .Distinct()
                        .ToList();

                    _logger.LogInformation(
                        "Total de moedas únicas encontradas: {total}",
                        codigosExternosUnicos.Count);

                    var moedasParaImportar = codigosExternosUnicos
                        .Where(codigo => !codigosExistentes.Contains(codigo))
                        .Select(codigo => new MoedaExternaDto
                        {
                            Codigo = codigo,
                            Nome = codigo 
                        })
                        .ToList();

                    if (moedasParaImportar.Any())
                    {
                        await noryxApi.ImportarMoedasAsync(moedasParaImportar);

                        _logger.LogInformation(
                            "Novas moedas realmente inseridas: {total}",
                            moedasParaImportar.Count);
                    }
                    else
                    {
                        _logger.LogInformation("Nenhuma moeda nova para importar.");
                    }

                    _logger.LogInformation("Sincronização de moedas concluída.");


                    var moedas = await noryxApi.BuscarMoedasAsync();
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
