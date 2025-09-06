using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Rental;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, SuccessResponseDto>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRentalCommandHandler(
        IRentalRepository rentalRepository,
        IDeliveryPersonRepository deliveryPersonRepository,
        IMotorcycleRepository motorcycleRepository,
        IUnitOfWork unitOfWork)
    {
        _rentalRepository = rentalRepository;
        _deliveryPersonRepository = deliveryPersonRepository;
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        // Validação usando FluentValidation
        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException(errorMessage);

        // Verifica se o entregador existe
        var entregador = await _deliveryPersonRepository.GetByIdentificadorAsync(request.EntregadorId);
        if (entregador == null)
            throw new ArgumentException("Entregador não encontrado");

        // Verifica se a moto existe (por enquanto usando ID como string)
        // TODO: implementar busca por identificador da moto
        var moto = await _motorcycleRepository.GetAllAsync();
        var motoEncontrada = moto.FirstOrDefault(m => m.Id.ToString().Contains(request.MotoId));
        if (motoEncontrada == null)
            throw new ArgumentException("Moto não encontrada");

        // Verifica se a moto está disponível
        if (motoEncontrada.Status != Domain.Enums.MotorcycleStatus.Available)
            throw new ArgumentException("Moto não está disponível para locação");

        // Cria a locação
        var rental = new Domain.Entities.Rental(
            request.EntregadorId,
            request.MotoId,
            request.DataInicio,
            request.DataTermino,
            request.DataPrevisaoTermino,
            motoEncontrada.DailyRate.Amount, // valor da diária
            request.Plano
        );

        // Marca a moto como alugada
        motoEncontrada.Rent();

        // Salva no banco
        await _rentalRepository.AddAsync(rental);
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Locação criada com sucesso" };
    }
}