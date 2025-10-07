using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.Rental;

public class GetRentalByIdQuery : IRequest<GetRentalResponseDto?>
{
    public Guid Id { get; set; }

    public GetRentalByIdQuery(Guid id)
    {
        Id = id;
    }
}