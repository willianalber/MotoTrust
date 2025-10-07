using MediatR;
using MassTransit;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Events;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Rental;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, SuccessResponseDto>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateRentalCommandHandler(
        IRentalRepository rentalRepository,
        IDeliveryPersonRepository deliveryPersonRepository,
        IMotorcycleRepository motorcycleRepository,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _rentalRepository = rentalRepository;
        _deliveryPersonRepository = deliveryPersonRepository;
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SuccessResponseDto> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException(errorMessage);

        var entregador = await _deliveryPersonRepository.GetByIdentificadorAsync(request.EntregadorId.ToString());
        if (entregador == null)
            throw new ArgumentException("Entregador não encontrado");

        if (entregador.TipoCNH != Domain.Enums.LicenseType.A && entregador.TipoCNH != Domain.Enums.LicenseType.AB)
            throw new ArgumentException("Somente entregadores com CNH tipo A ou AB podem alugar motos");

        var moto = await _motorcycleRepository.GetAllAsync();
        var motoEncontrada = moto.FirstOrDefault(m => m.Id.ToString().Contains(request.MotoId.ToString()));
        if (motoEncontrada == null)
            throw new ArgumentException("Moto não encontrada");

        if (motoEncontrada.Status != Domain.Enums.MotorcycleStatus.Available)
            throw new ArgumentException("Moto não está disponível para locação");

        var rental = new Domain.Entities.Rental(
            request.EntregadorId,
            request.MotoId,
            request.DataInicio,
            request.DataTermino,
            request.DataPrevisaoTermino,
            request.GetRentalPlan()
        );

        motoEncontrada.Rent();

        await _rentalRepository.AddAsync(rental);
        await _unitOfWork.SaveChangesAsync();

        var @event = new MotorcycleRentedEvent(rental.Id, rental.EntregadorId, rental.MotoId, 
            rental.DataInicio, rental.DataPrevisaoTermino);
        await _publishEndpoint.Publish(@event, cancellationToken);

        return new SuccessResponseDto { Mensagem = "Locação criada com sucesso" };
    }
}