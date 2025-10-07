using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Validators;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Application.Commands.Rental;

public class CreateRentalCommand : IRequest<SuccessResponseDto>
{
    public Guid EntregadorId { get; set; }
    public Guid MotoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public DateTime DataPrevisaoTermino { get; set; }
    public int Plano { get; set; }

    public RentalPlan GetRentalPlan()
    {
        return RentalPlanExtensions.FromDays(Plano);
    }

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