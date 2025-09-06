using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Validators;

namespace MotoTrust.Application.Commands.DeliveryPerson;

public class CreateDeliveryPersonCommand : IRequest<SuccessResponseDto>
{
    public string Identificador { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string NumeroCNH { get; set; } = string.Empty;
    public string TipoCNH { get; set; } = string.Empty;
    public string? ImagemCNH { get; set; }

    public string IsValid()
    {
        var validator = new CreateDeliveryPersonCommandValidator();
        var result = validator.Validate(this);
        
        if (result.IsValid)
            return string.Empty;
        
        var firstError = result.Errors.FirstOrDefault();
        return firstError?.ErrorMessage ?? "Dados inválidos";
    }
}
