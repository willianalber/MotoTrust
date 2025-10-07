using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Queries.DeliveryPerson;

public class GetAllDeliveryPersonsQuery : IRequest<List<GetDeliveryPersonResponseDto>>
{
}


