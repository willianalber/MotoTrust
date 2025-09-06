using FluentAssertions;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Tests.Domain.Entities;

public class RentalTests
{
    [Fact]
    public void CreateRental_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;

        // Act
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);

        // Assert
        rental.EntregadorId.Should().Be(entregadorId);
        rental.MotoId.Should().Be(motoId);
        rental.DataInicio.Should().Be(dataInicio);
        rental.DataTermino.Should().Be(dataTermino);
        rental.DataPrevisaoTermino.Should().Be(dataPrevisaoTermino);
        rental.ValorDiaria.Should().Be(valorDiaria);
        rental.Plano.Should().Be(plano);
        rental.Status.Should().Be(RentalStatus.Active);
    }

    [Fact]
    public void CompleteRental_WhenActive_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);

        // Act
        rental.Complete();

        // Assert
        rental.Status.Should().Be(RentalStatus.Completed);
    }

    [Fact]
    public void CancelRental_WhenActive_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);

        // Act
        rental.Cancel();

        // Assert
        rental.Status.Should().Be(RentalStatus.Cancelled);
    }

    [Fact]
    public void CompleteRental_WhenAlreadyCompleted_ShouldThrowException()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);
        rental.Complete();

        // Act & Assert
        var action = () => rental.Complete();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Apenas locações ativas podem ser finalizadas");
    }

    [Fact]
    public void CreateRental_WithPastDataInicio_ShouldThrowException()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(-1); // Data passada
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;

        // Act & Assert
        var action = () => new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Data de início não pode ser no passado*");
    }

    [Fact]
    public void CreateRental_WithInvalidValorDiaria_ShouldThrowException()
    {
        // Arrange
        var entregadorId = "entregador123";
        var motoId = "moto123";
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 0m; // Valor inválido
        var plano = 3;

        // Act & Assert
        var action = () => new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, valorDiaria, plano);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Valor da diária deve ser maior que zero*");
    }
}
