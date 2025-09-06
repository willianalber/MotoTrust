using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetMotorcyclesByPlateQuery : IRequest<List<GetMotorcycleResponseDto>>
{
    public string Placa { get; set; } = string.Empty;

    public GetMotorcyclesByPlateQuery(string placa)
    {
        Placa = placa;
    }
}
