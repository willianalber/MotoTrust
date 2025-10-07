using FluentValidation;
using MotoTrust.Application.Commands.DeliveryPerson;

namespace MotoTrust.Application.Validators;

public class CreateDeliveryPersonCommandValidator : AbstractValidator<CreateDeliveryPersonCommand>
{
    public CreateDeliveryPersonCommandValidator()
    {
        RuleFor(x => x.Identificador)
            .NotEmpty()
            .WithMessage("Identificador é obrigatório")
            .MaximumLength(100)
            .WithMessage("Identificador deve ter no máximo 100 caracteres");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MaximumLength(200)
            .WithMessage("Nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.CNPJ)
            .NotEmpty()
            .WithMessage("CNPJ é obrigatório")
            .Matches(@"^\d{14}$")
            .WithMessage("CNPJ deve conter exatamente 14 dígitos");

        RuleFor(x => x.DataNascimento)
            .NotEmpty()
            .WithMessage("Data de nascimento é obrigatória")
            .LessThan(DateTime.UtcNow.Date)
            .WithMessage("Data de nascimento deve ser no passado")
            .GreaterThan(DateTime.UtcNow.Date.AddYears(-100))
            .WithMessage("Data de nascimento inválida");

        RuleFor(x => x.NumeroCNH)
            .NotEmpty()
            .WithMessage("Número da CNH é obrigatório")
            .MaximumLength(20)
            .WithMessage("Número da CNH deve ter no máximo 20 caracteres");

        RuleFor(x => x.TipoCNH)
            .NotEmpty()
            .WithMessage("Tipo da CNH é obrigatório")
            .Matches(@"^(A|B|AB)$")
            .WithMessage("Tipo da CNH deve ser A, B ou AB");

        RuleFor(x => x.ImagemCNH)
            .NotEmpty()
            .WithMessage("Imagem da CNH é obrigatória")
            .When(x => !string.IsNullOrEmpty(x.ImagemCNH))
            .WithMessage("Imagem da CNH deve ser um base64 válido");
    }
}
