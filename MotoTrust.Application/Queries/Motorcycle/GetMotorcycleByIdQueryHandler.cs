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

        // Por enquanto retorna dados mockados baseados no ID
        // TODO: implementar busca real no banco quando tiver o campo identificador
        return new GetMotorcycleResponseDto
        {
            Identificador = $"moto{request.Id.ToString().Substring(0, 8)}", // mock
            Ano = motorcycle.Year,
            Modelo = motorcycle.Model,
            Placa = motorcycle.LicensePlate
        };
    }
}