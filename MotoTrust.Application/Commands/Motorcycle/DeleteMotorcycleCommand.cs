using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Commands.Motorcycle;

public class DeleteMotorcycleCommand : IRequest<SuccessResponseDto>
{
    public Guid Id { get; set; }

    public DeleteMotorcycleCommand(Guid id)
    {
        Id = id;
    }
}