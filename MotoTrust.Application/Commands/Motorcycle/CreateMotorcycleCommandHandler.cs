using MediatR;
using MassTransit;
using Microsoft.Extensions.Logging;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Events;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Application.Commands.Motorcycle;

public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, CreateMotorcycleResponseDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateMotorcycleCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateMotorcycleCommandHandler(
        IMotorcycleRepository motorcycleRepository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateMotorcycleCommandHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateMotorcycleResponseDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando criação de moto com identificador {Identificador}", request.Identificador);

        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _logger.LogWarning("Validação falhou para moto {Identificador}: {ErrorMessage}", request.Identificador, errorMessage);
            throw new ArgumentException(errorMessage);
        }

        // Valores padrão para campos não obrigatórios
        var motorcycle = new Domain.Entities.Motorcycle(
            "Mottu",
            request.Modelo,
            request.Ano,
            request.Placa,
            "Branca",
            150,
            new Money(100.00m, "BRL")
        );

        _logger.LogInformation("Criando moto {Modelo} {Ano} com placa {Placa}", request.Modelo, request.Ano, request.Placa);

        await _motorcycleRepository.AddAsync(motorcycle);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Moto criada com sucesso. ID: {MotorcycleId}", motorcycle.Id);

        // Publica evento de moto cadastrada
        var @event = new MotorcycleCreatedEvent(
            motorcycle.Id,
            request.Identificador,
            motorcycle.Year,
            motorcycle.Model,
            motorcycle.LicensePlate,
            motorcycle.CreatedAt
        );
        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Evento MotorcycleCreatedEvent publicado para moto {MotorcycleId}", motorcycle.Id);

        return new CreateMotorcycleResponseDto
        {
            Id = motorcycle.Id,
            Identificador = request.Identificador,
            Ano = motorcycle.Year,
            Modelo = motorcycle.Model,
            Placa = motorcycle.LicensePlate,
            CreatedAt = motorcycle.CreatedAt
        };
    }
}