using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Commands.Rental;

public class UpdateReturnDateCommandHandler : IRequestHandler<UpdateReturnDateCommand, UpdateReturnDateResponseDto>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReturnDateCommandHandler(IRentalRepository rentalRepository, IUnitOfWork unitOfWork)
    {
        _rentalRepository = rentalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateReturnDateResponseDto> Handle(UpdateReturnDateCommand request, CancellationToken cancellationToken)
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

        // Calcula valores antes de finalizar
        var valorTotal = rental.CalculateTotalValue(request.DataDevolucao);
        var diasUtilizados = (request.DataDevolucao - rental.DataInicio).Days + 1;
        var valorBase = diasUtilizados * rental.ValorDiaria;
        var valorMulta = valorTotal - valorBase;
        
        var diasAtraso = 0;
        var diasAntecipacao = 0;
        var tipoCalculo = "normal";
        
        if (request.DataDevolucao > rental.DataPrevisaoTermino)
        {
            diasAtraso = (request.DataDevolucao - rental.DataPrevisaoTermino).Days;
            tipoCalculo = "atrasado";
        }
        else if (request.DataDevolucao < rental.DataPrevisaoTermino)
        {
            diasAntecipacao = (rental.DataPrevisaoTermino - request.DataDevolucao).Days;
            tipoCalculo = "antecipado";
        }

        // Atualiza a data de devolução
        rental.CompleteRental(request.DataDevolucao);

        // Salva no banco
        await _unitOfWork.SaveChangesAsync();

        return new UpdateReturnDateResponseDto 
        { 
            Mensagem = "Data de devolução informada com sucesso",
            ValorTotal = valorTotal,
            ValorBase = valorBase,
            ValorMulta = valorMulta,
            DiasUtilizados = diasUtilizados,
            DiasAtraso = diasAtraso,
            DiasAntecipacao = diasAntecipacao,
            TipoCalculo = tipoCalculo
        };
    }
}
