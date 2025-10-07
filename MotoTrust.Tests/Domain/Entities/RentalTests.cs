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
        var entregadorId = Guid.NewGuid();
        var motoId = Guid.NewGuid();
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;

        // Act
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, RentalPlan.SevenDays);

        // Assert
        rental.EntregadorId.Should().Be(entregadorId);
        rental.MotoId.Should().Be(motoId);
        rental.DataInicio.Should().Be(dataInicio);
        rental.DataTermino.Should().Be(dataTermino);
        rental.DataPrevisaoTermino.Should().Be(dataPrevisaoTermino);
        rental.ValorDiaria.Should().Be(30.00m); // Valor do plano de 7 dias
        rental.Plano.Should().Be(7);
        rental.Status.Should().Be(RentalStatus.Active);
    }

    [Fact]
    public void CompleteRental_WhenActive_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var entregadorId = Guid.NewGuid();
        var motoId = Guid.NewGuid();
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, RentalPlan.SevenDays);

        // Act
        rental.Complete();

        // Assert
        rental.Status.Should().Be(RentalStatus.Completed);
    }

    [Fact]
    public void CancelRental_WhenActive_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var entregadorId = Guid.NewGuid();
        var motoId = Guid.NewGuid();
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, RentalPlan.SevenDays);

        // Act
        rental.Cancel();

        // Assert
        rental.Status.Should().Be(RentalStatus.Cancelled);
    }

    [Fact]
    public void CompleteRental_WhenAlreadyCompleted_ShouldThrowException()
    {
        // Arrange
        var entregadorId = Guid.NewGuid();
        var motoId = Guid.NewGuid();
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);
        var valorDiaria = 100.00m;
        var plano = 3;
        var rental = new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, RentalPlan.SevenDays);
        rental.Complete();

        // Act & Assert
        var action = () => rental.Complete();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Apenas locações ativas podem ser finalizadas");
    }

    [Fact]
    public void CreateRental_WithInvalidPlano_ShouldThrowException()
    {
        // Arrange
        var entregadorId = Guid.NewGuid();
        var motoId = Guid.NewGuid();
        var dataInicio = DateTime.UtcNow.AddDays(1);
        var dataTermino = DateTime.UtcNow.AddDays(3);
        var dataPrevisaoTermino = DateTime.UtcNow.AddDays(3);

        // Act & Assert - Usando um valor inválido para o enum (99 dias)
        var action = () => new Rental(entregadorId, motoId, dataInicio, dataTermino, dataPrevisaoTermino, (RentalPlan)99);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Plano inválido*");
    }
}
