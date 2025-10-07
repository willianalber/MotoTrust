using FluentValidation;
using MotoTrust.Application.Commands.Rental;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Application.Validators;

public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
{
    public CreateRentalCommandValidator()
    {
        RuleFor(x => x.EntregadorId)
            .NotEmpty()
            .WithMessage("ID do entregador é obrigatório");

        RuleFor(x => x.MotoId)
            .NotEmpty()
            .WithMessage("ID da moto é obrigatório");

        RuleFor(x => x.DataInicio)
            .NotEmpty()
            .WithMessage("Data de início é obrigatória")
            .Equal(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Data de início deve ser exatamente o primeiro dia após a data de criação");

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
            .Must(plano => RentalPlanExtensions.IsValidPlan(plano))
            .WithMessage("Plano inválido. Planos válidos: 7, 15, 30, 45, 50 dias");
    }
}
