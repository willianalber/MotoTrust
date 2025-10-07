using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetMotorcyclesByPlateQueryHandler : IRequestHandler<GetMotorcyclesByPlateQuery, List<GetMotorcycleResponseDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public GetMotorcyclesByPlateQueryHandler(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<List<GetMotorcycleResponseDto>> Handle(GetMotorcyclesByPlateQuery request, CancellationToken cancellationToken)
    {
        var motorcycles = await _motorcycleRepository.GetMotorcyclesByPlateFilterAsync(request.Placa);
        
        var result = motorcycles
            .Select(m => new GetMotorcycleResponseDto
            {
                Identificador = m.Id.ToString(),
                Ano = m.Year,
                Modelo = m.Model,
                Placa = m.LicensePlate
            })
            .ToList();

        return result;
    }
}
