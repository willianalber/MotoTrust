using MediatR;
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

    public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateMotorcycleResponseDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        // Validação usando FluentValidation
        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException(errorMessage);

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

        // Salva no banco
        await _motorcycleRepository.AddAsync(motorcycle);
        await _unitOfWork.SaveChangesAsync();

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