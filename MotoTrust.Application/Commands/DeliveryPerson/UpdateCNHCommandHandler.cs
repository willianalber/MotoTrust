using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Interfaces;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.DeliveryPerson;

public class UpdateCNHCommandHandler : IRequestHandler<UpdateCNHCommand, SuccessResponseDto>
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageStorageService _imageStorageService;

    public UpdateCNHCommandHandler(
        IDeliveryPersonRepository deliveryPersonRepository, 
        IUnitOfWork unitOfWork,
        IImageStorageService imageStorageService)
    {
        _deliveryPersonRepository = deliveryPersonRepository;
        _unitOfWork = unitOfWork;
        _imageStorageService = imageStorageService;
    }

    public async Task<SuccessResponseDto> Handle(UpdateCNHCommand request, CancellationToken cancellationToken)
    {
        // Validações básicas
        if (string.IsNullOrWhiteSpace(request.ImagemCNH))
            throw new ArgumentException("Imagem da CNH é obrigatória");

        // Busca o entregador
        var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(request.Id);
        if (deliveryPerson == null)
            throw new ArgumentException("Entregador não encontrado");

        // Remove a imagem antiga se existir
        if (!string.IsNullOrEmpty(deliveryPerson.ImagemCNH))
        {
            await _imageStorageService.DeleteImageAsync(deliveryPerson.ImagemCNH);
        }

        // Salva a nova imagem
        var imageFileName = await _imageStorageService.SaveImageAsync(request.ImagemCNH, $"cnh_{deliveryPerson.Identificador}");

        // Atualiza a imagem da CNH
        deliveryPerson.UpdateCNHImage(imageFileName);

        // Salva no banco
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "CNH enviada com sucesso" };
    }
}
