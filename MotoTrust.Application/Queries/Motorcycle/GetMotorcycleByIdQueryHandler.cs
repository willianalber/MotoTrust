using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Domain.Interfaces;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, GetMotorcycleResponseDto?>
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<GetMotorcycleResponseDto?> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
        
        if (motorcycle == null)
            return null;

        return new GetMotorcycleResponseDto
        {
            Identificador = request.Id.ToString(),
            Ano = motorcycle.Year,
            Modelo = motorcycle.Model,
            Placa = motorcycle.LicensePlate
        };
    }
}