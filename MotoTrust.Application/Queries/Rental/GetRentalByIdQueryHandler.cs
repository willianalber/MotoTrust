using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.Rental;

public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, GetRentalResponseDto?>
{
    private readonly IRentalRepository _rentalRepository;

    public GetRentalByIdQueryHandler(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task<GetRentalResponseDto?> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        var rental = await _rentalRepository.GetByIdAsync(request.Id);
        
        if (rental == null)
            return null;

        return new GetRentalResponseDto
        {
            Identificador = rental.Id.ToString(),
            ValorDiaria = rental.ValorDiaria,
            EntregadorId = rental.EntregadorId.ToString(),
            MotoId = rental.MotoId.ToString(),
            DataInicio = rental.DataInicio,
            DataTermino = rental.DataTermino,
            DataPrevisaoTermino = rental.DataPrevisaoTermino,
            DataDevolucao = rental.DataDevolucao
        };
    }
}