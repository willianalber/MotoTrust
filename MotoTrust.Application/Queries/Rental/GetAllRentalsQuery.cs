using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.Rental;

public class GetAllRentalsQuery : IRequest<List<GetRentalResponseDto>>
{
}


