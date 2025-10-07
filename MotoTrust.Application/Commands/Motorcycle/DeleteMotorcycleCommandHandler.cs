using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Motorcycle;

public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, SuccessResponseDto>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (motorcycle == null)
            throw new ArgumentException("Moto não encontrada");

        if (motorcycle.Status == Domain.Enums.MotorcycleStatus.Rented)
            throw new ArgumentException("Não é possível remover uma moto que está alugada");

        motorcycle.Delete();

        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Moto removida com sucesso" };
    }
}