using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetMotorcycleByIdQuery : IRequest<GetMotorcycleResponseDto?>
{
    public Guid Id { get; set; }

    public GetMotorcycleByIdQuery(Guid id)
    {
        Id = id;
    }
}