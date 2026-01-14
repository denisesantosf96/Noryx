using Noryx.API.Application.Services;
using WorkerIntegracao;
using WorkerIntegracao.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IApiAwesome, ApiAwesome>();
builder.Services.AddScoped<IApiNoryx, ApiNoryx>();

var host = builder.Build();
host.Run();
