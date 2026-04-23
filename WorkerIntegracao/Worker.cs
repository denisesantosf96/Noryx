using System.Globalization;
using Noryx.API.Application.Dtos;
using WorkerIntegracao.Services;

namespace WorkerIntegracao
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        bool _usarAwesomeApi = true;
        bool _buscaMoedas = false;
        bool _buscaCotacoes = true;

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
                using var scope = _scopeFactory.CreateScope();

                var awesomeApi = scope.ServiceProvider.GetRequiredService<IApiAwesome>();
                var noryxApi = scope.ServiceProvider.GetRequiredService<IApiNoryx>();

                try
                {
                    if (_usarAwesomeApi)
                    {
                        if (_buscaMoedas)
                            await IntegracaoMoedasAsync(noryxApi, awesomeApi);

                        if (_buscaCotacoes)
                            await IntegracaoCotacoesAsync(noryxApi, awesomeApi);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro geral no Worker Noryx.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task IntegracaoMoedasAsync(IApiNoryx noryxApi, IApiAwesome awesomeApi)
        {
            var moedasExistentes = await noryxApi.BuscarMoedasAsync();
            var codigosExistentes = moedasExistentes
                .Select(m => m.Codigo)
                .ToHashSet();

            var moedasExternas = await awesomeApi.BuscarMoedasAsync();

            var codigosExternos = moedasExternas
                .Select(m => m.Key.Split('-')[0])
                .Distinct()
                .ToList();

            var novasMoedas = codigosExternos
                .Where(codigo => !codigosExistentes.Contains(codigo))
                .Select(codigo => new MoedaExternaDto
                {
                    Codigo = codigo,
                    Nome = codigo
                })
                .ToList();

            if (novasMoedas.Any())
            {
                await noryxApi.ImportarMoedasAsync(novasMoedas);
                _logger.LogInformation("Moedas inseridas: {total}", novasMoedas.Count);
            }
        }

        private async Task IntegracaoCotacoesAsync(IApiNoryx noryxApi, IApiAwesome awesomeApi)
        {
            var moedas = await noryxApi.BuscarMoedasAsync();
            const string moedaDestino = "BRL";

            foreach (var moeda in moedas)
            {
                if (moeda.Codigo == moedaDestino)
                    continue;

                try
                {
                    var cotacaoApi = await awesomeApi
                        .BuscarCotacaoAsync(moeda.Codigo, moedaDestino);

                    if (cotacaoApi == null)
                        continue;

                    var valor = decimal.Parse(
                        cotacaoApi.bid,
                        CultureInfo.InvariantCulture);

                    var dto = new CotacaoDto
                    {
                        MoedaOrigem = moeda.Codigo,
                        MoedaDestino = moedaDestino,
                        Valor = valor
                    };

                    await noryxApi.InserirCotacaoAsync(dto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Erro ao processar moeda {moeda}",
                        moeda.Codigo);
                }
            }

            _logger.LogInformation("Integração de cotações concluída.");
        }
    }
}