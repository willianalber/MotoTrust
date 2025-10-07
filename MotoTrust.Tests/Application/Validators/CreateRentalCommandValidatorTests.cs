using FluentAssertions;
using MotoTrust.Application.Commands.Rental;
using MotoTrust.Application.Validators;

namespace MotoTrust.Tests.Application.Validators;

public class CreateRentalCommandValidatorTests
{
    private readonly CreateRentalCommandValidator _validator;

    public CreateRentalCommandValidatorTests()
    {
        _validator = new CreateRentalCommandValidator();
    }

    [Fact]
    public void Validate_WithEmptyEntregadorId_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateRentalCommand
        {
            EntregadorId = Guid.Empty,
            MotoId = Guid.NewGuid(),
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataTermino = DateTime.UtcNow.AddDays(3),
            DataPrevisaoTermino = DateTime.UtcNow.AddDays(3),
            Plano = 3
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "EntregadorId");
    }

    [Fact]
    public void Validate_WithEmptyMotoId_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateRentalCommand
        {
            EntregadorId = Guid.NewGuid(),
            MotoId = Guid.Empty,
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataTermino = DateTime.UtcNow.AddDays(3),
            DataPrevisaoTermino = DateTime.UtcNow.AddDays(3),
            Plano = 3
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MotoId");
    }

    [Fact]
    public void Validate_WithPastDataInicio_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateRentalCommand
        {
            EntregadorId = Guid.NewGuid(),
            MotoId = Guid.NewGuid(),
            DataInicio = DateTime.UtcNow.AddDays(-1), // Data passada
            DataTermino = DateTime.UtcNow.AddDays(3),
            DataPrevisaoTermino = DateTime.UtcNow.AddDays(3),
            Plano = 3
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DataInicio");
    }

    [Fact]
    public void Validate_WithInvalidPlano_ShouldReturnInvalid()
    {
        // Arrange
        var command = new CreateRentalCommand
        {
            EntregadorId = Guid.NewGuid(),
            MotoId = Guid.NewGuid(),
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataTermino = DateTime.UtcNow.AddDays(3),
            DataPrevisaoTermino = DateTime.UtcNow.AddDays(3),
            Plano = 0 // Plano inválido
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Plano");
    }
}