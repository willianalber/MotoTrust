using MassTransit;
using Microsoft.Extensions.Logging;
using MotoTrust.Domain.Events;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Consumer.Consumers;

public class MotorcycleCreatedConsumer : IConsumer<MotorcycleCreatedEvent>
{
    private readonly ILogger<MotorcycleCreatedConsumer> _logger;
    private readonly IMotorcycleNotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MotorcycleCreatedConsumer(
        ILogger<MotorcycleCreatedConsumer> logger,
        IMotorcycleNotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<MotorcycleCreatedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("MotorcycleCreatedEvent recebido - Moto {MotorcycleId} cadastrada: {Modelo} {Ano} - Placa: {Placa}",
            @event.MotorcycleId, @event.Modelo, @event.Ano, @event.Placa);

        try
        {
            // Verifica se é uma moto do ano 2024
            if (@event.Ano == 2024)
            {
                _logger.LogInformation("Moto 2024 detectada! Criando notificação especial para {MotorcycleId}", @event.MotorcycleId);

                var notification = new Domain.Entities.MotorcycleNotification(
                    @event.MotorcycleId,
                    @event.Identificador,
                    @event.Ano,
                    @event.Modelo,
                    @event.Placa,
                    "MOTO_2024_CADASTRADA",
                    $"Nova moto modelo {@event.Modelo} do ano 2024 foi cadastrada com placa {@event.Placa}",
                    @event.CreatedAt
                );

                await _notificationRepository.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Notificação para moto 2024 salva no banco de dados - ID: {NotificationId}", notification.Id);
            }
            else
            {
                _logger.LogInformation("Moto {Ano} cadastrada - não é 2024, apenas logando", @event.Ano);
            }

            _logger.LogInformation("MotorcycleCreatedEvent processado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar MotorcycleCreatedEvent para moto {MotorcycleId}", @event.MotorcycleId);
            throw; // Re-throw para que o MassTransit possa tentar novamente
        }
    }
}
