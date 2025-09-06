using MediatR;
using Microsoft.Extensions.Logging;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Application.Commands.Motorcycle;

public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, CreateMotorcycleResponseDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateMotorcycleCommandHandler> _logger;

    public CreateMotorcycleCommandHandler(
        IMotorcycleRepository motorcycleRepository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateMotorcycleCommandHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateMotorcycleResponseDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando criação de moto com identificador {Identificador}", request.Identificador);

        // Validação usando FluentValidation
        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _logger.LogWarning("Validação falhou para moto {Identificador}: {ErrorMessage}", request.Identificador, errorMessage);
            throw new ArgumentException(errorMessage);
        }

        // Cria a moto com valores padrão
        var motorcycle = new Domain.Entities.Motorcycle(
            "Mottu", // marca padrão
            request.Modelo,
            request.Ano,
            request.Placa,
            "Branca", // cor padrão
            150, // cilindrada padrão
            new Money(100.00m, "BRL") // preço diário padrão
        );

        _logger.LogInformation("Criando moto {Modelo} {Ano} com placa {Placa}", request.Modelo, request.Ano, request.Placa);

        // Salva no banco
        await _motorcycleRepository.AddAsync(motorcycle);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Moto criada com sucesso. ID: {MotorcycleId}", motorcycle.Id);

        // Retorna o DTO de resposta
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