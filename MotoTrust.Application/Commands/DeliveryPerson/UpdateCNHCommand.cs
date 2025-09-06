using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Commands.DeliveryPerson;

public class UpdateCNHCommand : IRequest<SuccessResponseDto>
{
    public Guid Id { get; set; }
    public string ImagemCNH { get; set; } = string.Empty;
}
