using FluentValidation;
using MotoTrust.Application.Commands.Rental;

namespace MotoTrust.Application.Validators;

public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
{
    public CreateRentalCommandValidator()
    {
        RuleFor(x => x.EntregadorId)
            .NotEmpty()
            .WithMessage("ID do entregador é obrigatório")
            .MaximumLength(100)
            .WithMessage("ID do entregador deve ter no máximo 100 caracteres");

        RuleFor(x => x.MotoId)
            .NotEmpty()
            .WithMessage("ID da moto é obrigatório")
            .MaximumLength(100)
            .WithMessage("ID da moto deve ter no máximo 100 caracteres");

        RuleFor(x => x.DataInicio)
            .NotEmpty()
            .WithMessage("Data de início é obrigatória")
            .GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("Data de início deve ser no futuro");

        RuleFor(x => x.DataTermino)
            .NotEmpty()
            .WithMessage("Data de término é obrigatória")
            .GreaterThan(x => x.DataInicio)
            .WithMessage("Data de término deve ser posterior à data de início");

        RuleFor(x => x.DataPrevisaoTermino)
            .NotEmpty()
            .WithMessage("Data de previsão de término é obrigatória")
            .GreaterThanOrEqualTo(x => x.DataInicio)
            .WithMessage("Data de previsão de término deve ser posterior ou igual à data de início");

        RuleFor(x => x.Plano)
            .GreaterThan(0)
            .WithMessage("Plano deve ser maior que zero")
            .LessThanOrEqualTo(365)
            .WithMessage("Plano deve ser menor ou igual a 365 dias");
    }
}
