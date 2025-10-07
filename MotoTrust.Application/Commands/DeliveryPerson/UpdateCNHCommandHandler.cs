using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.DeliveryPerson;

public class UpdateCNHCommandHandler : IRequestHandler<UpdateCNHCommand, SuccessResponseDto>
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCNHCommandHandler(
        IDeliveryPersonRepository deliveryPersonRepository, 
        IUnitOfWork unitOfWork)
    {
        _deliveryPersonRepository = deliveryPersonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(UpdateCNHCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ImagemCNH))
            throw new ArgumentException("Imagem da CNH é obrigatória");

        var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(request.Id);
        if (deliveryPerson == null)
            throw new ArgumentException("Entregador não encontrado");

        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "CNH enviada com sucesso" };
    }
}
