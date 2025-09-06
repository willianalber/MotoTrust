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
        // Busca otimizada no repositório
        var motorcycles = await _motorcycleRepository.GetMotorcyclesByPlateFilterAsync(request.Placa);
        
        var result = motorcycles
            .Select(m => new GetMotorcycleResponseDto
            {
                Identificador = $"moto{m.Id.ToString().Substring(0, 8)}", // mock
                Ano = m.Year,
                Modelo = m.Model,
                Placa = m.LicensePlate
            })
            .ToList();

        return result;
    }
}
