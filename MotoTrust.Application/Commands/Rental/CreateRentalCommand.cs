using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Validators;

namespace MotoTrust.Application.Commands.Rental;

public class CreateRentalCommand : IRequest<SuccessResponseDto>
{
    public string EntregadorId { get; set; } = string.Empty;
    public string MotoId { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public DateTime DataPrevisaoTermino { get; set; }
    public int Plano { get; set; }

    public string IsValid()
    {
        var validator = new CreateRentalCommandValidator();
        var result = validator.Validate(this);
        
        if (result.IsValid)
            return string.Empty;
        
        var firstError = result.Errors.FirstOrDefault();
        return firstError?.ErrorMessage ?? "Dados inválidos";
    }
}