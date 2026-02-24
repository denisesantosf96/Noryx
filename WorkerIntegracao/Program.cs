using Noryx.API.Application.Services;
using WorkerIntegracao;
using WorkerIntegracao.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IApiAwesome, ApiAwesome>();
builder.Services.AddSingleton<IApiNoryx, ApiNoryx>();

var host = builder.Build();
host.Run();
