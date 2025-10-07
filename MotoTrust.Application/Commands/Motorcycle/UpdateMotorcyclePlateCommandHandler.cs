using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Motorcycle;

public class UpdateMotorcyclePlateCommandHandler : IRequestHandler<UpdateMotorcyclePlateCommand, SuccessResponseDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMotorcyclePlateCommandHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(UpdateMotorcyclePlateCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Placa))
            throw new ArgumentException("Placa é obrigatória");

        var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (motorcycle == null)
            throw new ArgumentException("Moto não encontrada");

        motorcycle.UpdateLicensePlate(request.Placa);

        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Placa modificada com sucesso" };
    }
}
