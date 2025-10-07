using MediatR;
using MotoTrust.Application.DTOs;
using MotoTrust.Application.Validators;

namespace MotoTrust.Application.Commands.Motorcycle;

public class CreateMotorcycleCommand : IRequest<CreateMotorcycleResponseDto>
{
    public string Identificador { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int CapacidadeMotor { get; set; }
    public decimal ValorDiaria { get; set; }

    public string IsValid()
    {
        var validator = new CreateMotorcycleCommandValidator();
        var result = validator.Validate(this);
        
        if (result.IsValid)
            return string.Empty;
        
        var firstError = result.Errors.FirstOrDefault();
        return firstError?.ErrorMessage ?? "Dados inválidos";
    }
}