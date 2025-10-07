using MediatR;
using MotoTrust.Application.DTOs;

namespace MotoTrust.Application.Commands.Motorcycle;

public class UpdateMotorcyclePlateCommand : IRequest<SuccessResponseDto>
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
}
