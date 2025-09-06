using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.DeliveryPerson;

public class CreateDeliveryPersonCommandHandler : IRequestHandler<CreateDeliveryPersonCommand, SuccessResponseDto>
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeliveryPersonCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, IUnitOfWork unitOfWork)
    {
        _deliveryPersonRepository = deliveryPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(CreateDeliveryPersonCommand request, CancellationToken cancellationToken)
    {
        var errorMessage = request.IsValid();
        if (!string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException(errorMessage);

        if (!Enum.TryParse<LicenseType>(request.TipoCNH, out var tipoCNH))
            throw new ArgumentException("Tipo de CNH inválido");

        var existingDeliveryPerson = await _deliveryPersonRepository.GetByIdentificadorAsync(request.Identificador);
        if (existingDeliveryPerson != null)
            throw new ArgumentException("Já existe um entregador com esse identificador");

        var deliveryPerson = new Domain.Entities.DeliveryPerson(
            request.Identificador,
            request.Nome,
            request.CNPJ,
            request.DataNascimento,
            request.NumeroCNH,
            tipoCNH,
            request.ImagemCNH
        );

        await _deliveryPersonRepository.AddAsync(deliveryPerson);
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Entregador cadastrado com sucesso" };
    }
}
