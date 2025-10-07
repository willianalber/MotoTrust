using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.Motorcycle;

public class GetAllMotorcyclesQuery : IRequest<List<GetMotorcycleResponseDto>>
{
}


