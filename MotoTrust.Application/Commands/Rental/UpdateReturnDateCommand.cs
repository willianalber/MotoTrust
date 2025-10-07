using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Commands.Rental;

public class UpdateReturnDateCommand : IRequest<UpdateReturnDateResponseDto>
{
    public Guid Id { get; set; }
    public DateTime DataDevolucao { get; set; }
}
