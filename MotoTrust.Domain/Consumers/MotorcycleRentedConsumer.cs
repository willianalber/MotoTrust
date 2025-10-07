using MassTransit;
using Microsoft.Extensions.Logging;
using MotoTrust.Domain.Events;

namespace MotoTrust.Consumer.Consumers;

public class MotorcycleRentedConsumer : IConsumer<MotorcycleRentedEvent>
{
    private readonly ILogger<MotorcycleRentedConsumer> _logger;

    public MotorcycleRentedConsumer(ILogger<MotorcycleRentedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MotorcycleRentedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("MotorcycleRentedEvent processado - Moto {MotoId} alugada pelo entregador {EntregadorId} em {DataInicio:dd/MM/yyyy HH:mm}. Previsão de término: {DataPrevisaoTermino:dd/MM/yyyy HH:mm}. Rental ID: {RentalId}",
            @event.MotoId, @event.EntregadorId, @event.DataInicio, @event.DataPrevisaoTermino, @event.RentalId);

        // Aqui poderia implementar várias ações como:
        // - Envio de email de confirmação
        // - Notificação push para o entregador
        // - Integração com GPS
        // - Auditoria
        // - Integração com sistema de pagamento

        _logger.LogInformation("Evento de locação processado com sucesso");

        return Task.CompletedTask;
    }
}
