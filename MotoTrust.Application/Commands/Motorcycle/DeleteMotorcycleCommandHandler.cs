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
        // Busca a moto
        var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
        if (motorcycle == null)
            throw new ArgumentException("Moto não encontrada");

        // Verifica se a moto pode ser removida (não pode estar alugada)
        if (motorcycle.Status == Domain.Enums.MotorcycleStatus.Rented)
            throw new ArgumentException("Não é possível remover uma moto que está alugada");

        // Exclusão lógica - marca como inativa
        motorcycle.Delete();

        // Salva no banco
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Moto removida com sucesso" };
    }
}