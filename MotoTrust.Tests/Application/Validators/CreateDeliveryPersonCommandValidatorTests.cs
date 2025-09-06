using FluentAssertions;
using MotoTrust.Application.Commands.DeliveryPerson;
using MotoTrust.Application.Validators;

namespace MotoTrust.Tests.Application.Validators;

public class CreateDeliveryPersonCommandValidatorTests
{
    private readonly CreateDeliveryPersonCommandValidator _validator;

    public CreateDeliveryPersonCommandValidatorTests()
    {
        _validator = new CreateDeliveryPersonCommandValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldReturnValid()
    {
        // Arrange
        var command = new CreateDeliveryPersonCommand
        {
            Identificador = "entregador123",
            Nome = "João da Silva",
            CNPJ = "12345678901234",
            DataNascimento = new DateTime(1990, 1, 1),
            NumeroCNH = "12345678900",
            TipoCNH = "A"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyIdentificador_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateDeliveryPersonCommand
        {
            Identificador = "",
            Nome = "João da Silva",
            CNPJ = "12345678901234",
            DataNascimento = new DateTime(1990, 1, 1),
            NumeroCNH = "12345678900",
            TipoCNH = "A"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Identificador");
    }

    [Fact]
    public void Validate_WithEmptyNome_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateDeliveryPersonCommand
        {
            Identificador = "entregador123",
            Nome = "",
            CNPJ = "12345678901234",
            DataNascimento = new DateTime(1990, 1, 1),
            NumeroCNH = "12345678900",
            TipoCNH = "A"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Nome");
    }

    [Fact]
    public void Validate_WithInvalidCNPJ_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateDeliveryPersonCommand
        {
            Identificador = "entregador123",
            Nome = "João da Silva",
            CNPJ = "123456789", // CNPJ muito curto
            DataNascimento = new DateTime(1990, 1, 1),
            NumeroCNH = "12345678900",
            TipoCNH = "A"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CNPJ");
    }

    [Fact]
    public void Validate_WithEmptyNumeroCNH_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateDeliveryPersonCommand
        {
            Identificador = "entregador123",
            Nome = "João da Silva",
            CNPJ = "12345678901234",
            DataNascimento = new DateTime(1990, 1, 1),
            NumeroCNH = "",
            TipoCNH = "A"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NumeroCNH");
    }
}