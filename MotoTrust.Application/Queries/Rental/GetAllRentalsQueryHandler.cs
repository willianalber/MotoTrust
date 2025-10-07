using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.Rental;

public class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, List<GetRentalResponseDto>>
{
    private readonly IRentalRepository _rentalRepository;

    public GetAllRentalsQueryHandler(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task<List<GetRentalResponseDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
    {
        var rentals = await _rentalRepository.GetAllAsync();
        
        var result = rentals
            .Where(r => r.IsActive)
            .Select(r => new GetRentalResponseDto
            {
                Identificador = r.Id.ToString(),
                ValorDiaria = r.ValorDiaria,
                EntregadorId = r.EntregadorId.ToString(),
                MotoId = r.MotoId.ToString(),
                DataInicio = r.DataInicio,
                DataTermino = r.DataTermino,
                DataPrevisaoTermino = r.DataPrevisaoTermino,
                DataDevolucao = r.DataDevolucao
            })
            .OrderByDescending(r => r.DataInicio)
            .ToList();

        return result;
    }
}


