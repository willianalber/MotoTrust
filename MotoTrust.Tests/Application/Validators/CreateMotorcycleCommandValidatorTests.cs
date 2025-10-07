using FluentAssertions;
using MotoTrust.Application.Commands.Motorcycle;
using MotoTrust.Application.Validators;

namespace MotoTrust.Tests.Application.Validators;

public class CreateMotorcycleCommandValidatorTests
{
    private readonly CreateMotorcycleCommandValidator _validator;

    public CreateMotorcycleCommandValidatorTests()
    {
        _validator = new CreateMotorcycleCommandValidator();
    }

    [Fact]
    public void Validate_WithEmptyIdentificador_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateMotorcycleCommand
        {
            Identificador = "",
            Ano = 2023,
            Modelo = "Honda CB 600F",
            Placa = "ABC-1234"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Identificador");
    }

    [Fact]
    public void Validate_WithInvalidAno_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateMotorcycleCommand
        {
            Identificador = "moto123",
            Ano = 1800, // Ano muito antigo
            Modelo = "Honda CB 600F",
            Placa = "ABC-1234"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Ano");
    }

    [Fact]
    public void Validate_WithEmptyModelo_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateMotorcycleCommand
        {
            Identificador = "moto123",
            Ano = 2023,
            Modelo = "",
            Placa = "ABC-1234"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Modelo");
    }

    [Fact]
    public void Validate_WithEmptyPlaca_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateMotorcycleCommand
        {
            Identificador = "moto123",
            Ano = 2023,
            Modelo = "Honda CB 600F",
            Placa = ""
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Placa");
    }
}