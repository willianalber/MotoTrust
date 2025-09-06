using FluentValidation;
using MotoTrust.Application.Commands.Motorcycle;

namespace MotoTrust.Application.Validators;

public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
{
    public CreateMotorcycleCommandValidator()
    {
        RuleFor(x => x.Identificador)
            .NotEmpty()
            .WithMessage("Identificador é obrigatório")
            .MaximumLength(100)
            .WithMessage("Identificador deve ter no máximo 100 caracteres");

        RuleFor(x => x.Ano)
            .GreaterThan(1900)
            .WithMessage("Ano deve ser maior que 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("Ano não pode ser muito no futuro");

        RuleFor(x => x.Modelo)
            .NotEmpty()
            .WithMessage("Modelo é obrigatório")
            .MaximumLength(100)
            .WithMessage("Modelo deve ter no máximo 100 caracteres");

        RuleFor(x => x.Placa)
            .NotEmpty()
            .WithMessage("Placa é obrigatória")
            .Matches(@"^[A-Z]{3}-\d{4}$")
            .WithMessage("Placa deve estar no formato ABC-1234");
    }
}
