using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Tests;


public class MotorcycleTests
{
    [Fact]
    public void CreateMotorcycle_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var brand = "Honda";
        var model = "CB 600F";
        var year = 2023;
        var licensePlate = "ABC1234";
        var color = "Vermelha";
        var engineCapacity = 600;
        var dailyRate = new Money(150.00m, "BRL");

        // Act
        var motorcycle = new Motorcycle(brand, model, year, licensePlate, color, engineCapacity, dailyRate);

        // Assert
        Assert.Equal(brand, motorcycle.Brand);
        Assert.Equal(model, motorcycle.Model);
        Assert.Equal(year, motorcycle.Year);
        Assert.Equal(licensePlate, motorcycle.LicensePlate);
        Assert.Equal(color, motorcycle.Color);
        Assert.Equal(engineCapacity, motorcycle.EngineCapacity);
        Assert.Equal(dailyRate.Amount, motorcycle.DailyRate.Amount);
        Assert.Equal(MotorcycleStatus.Available, motorcycle.Status);
    }

    [Fact]
    public void RentMotorcycle_WhenAvailable_ShouldChangeStatusToRented()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));

        // Act
        motorcycle.Rent();

        // Assert
        Assert.Equal(MotorcycleStatus.Rented, motorcycle.Status);
    }

    [Fact]
    public void RentMotorcycle_WhenNotAvailable_ShouldThrowException()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));
        motorcycle.Rent(); // Primeiro aluguel

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => motorcycle.Rent());
    }
}

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
        Assert.Equal(entregadorId, rental.EntregadorId);
        Assert.Equal(motoId, rental.MotoId);
        Assert.Equal(dataInicio, rental.DataInicio);
        Assert.Equal(dataTermino, rental.DataTermino);
        Assert.Equal(dataPrevisaoTermino, rental.DataPrevisaoTermino);
        Assert.Equal(valorDiaria, rental.ValorDiaria);
        Assert.Equal(plano, rental.Plano);
        Assert.Equal(RentalStatus.Active, rental.Status);
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
        Assert.Equal(RentalStatus.Completed, rental.Status);
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
        Assert.Equal(RentalStatus.Cancelled, rental.Status);
    }
}
