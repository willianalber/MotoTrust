using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotoTrust.Consumer.Consumers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MotorcycleRentedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = context.GetRequiredService<IConfiguration>().GetSection("RabbitMQ");
                cfg.Host(rabbitConfig["Host"], "/", h =>
                {
                    h.Username(rabbitConfig["Username"]);
                    h.Password(rabbitConfig["Password"]);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Iniciando MotoTrust Consumer - Sistema de eventos via RabbitMQ");
    logger.LogInformation("Aguardando eventos de locação de motos...");
    
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Consumer terminou inesperadamente - algo deu errado");
}