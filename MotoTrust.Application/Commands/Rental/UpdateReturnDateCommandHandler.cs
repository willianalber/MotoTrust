using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Rental;

public class UpdateReturnDateCommandHandler : IRequestHandler<UpdateReturnDateCommand, SuccessResponseDto>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReturnDateCommandHandler(IRentalRepository rentalRepository, IUnitOfWork unitOfWork)
    {
        _rentalRepository = rentalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SuccessResponseDto> Handle(UpdateReturnDateCommand request, CancellationToken cancellationToken)
    {
        // Validações básicas
        if (request.DataDevolucao < DateTime.UtcNow.AddDays(-1))
            throw new ArgumentException("Data de devolução não pode ser muito antiga");

        // Busca a locação
        var rental = await _rentalRepository.GetByIdAsync(request.Id);
        if (rental == null)
            throw new ArgumentException("Locação não encontrada");

        // Verifica se a locação está ativa
        if (rental.Status != Domain.Enums.RentalStatus.Active)
            throw new ArgumentException("Apenas locações ativas podem ter data de devolução informada");

        // Atualiza a data de devolução
        rental.CompleteRental(request.DataDevolucao);

        // Salva no banco
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResponseDto { Mensagem = "Data de devolução informada com sucesso" };
    }
}
