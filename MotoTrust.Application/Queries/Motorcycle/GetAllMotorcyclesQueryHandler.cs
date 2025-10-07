using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetAllMotorcyclesQueryHandler : IRequestHandler<GetAllMotorcyclesQuery, List<GetMotorcycleResponseDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public GetAllMotorcyclesQueryHandler(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<List<GetMotorcycleResponseDto>> Handle(GetAllMotorcyclesQuery request, CancellationToken cancellationToken)
    {
        var motorcycles = await _motorcycleRepository.GetAllAsync();
        
        var result = motorcycles
            .Where(m => m.IsActive)
            .Select(m => new GetMotorcycleResponseDto
            {
                Identificador = m.Id.ToString(),
                Ano = m.Year,
                Modelo = m.Model,
                Placa = m.LicensePlate
            })
            .OrderBy(m => m.Modelo)
            .ThenBy(m => m.Ano)
            .ToList();

        return result;
    }
}
