using FluentAssertions;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Tests.Domain.Entities;

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
        motorcycle.Brand.Should().Be(brand);
        motorcycle.Model.Should().Be(model);
        motorcycle.Year.Should().Be(year);
        motorcycle.LicensePlate.Should().Be(licensePlate);
        motorcycle.Color.Should().Be(color);
        motorcycle.EngineCapacity.Should().Be(engineCapacity);
        motorcycle.DailyRate.Amount.Should().Be(dailyRate.Amount);
        motorcycle.Status.Should().Be(MotorcycleStatus.Available);
    }

    [Fact]
    public void RentMotorcycle_WhenAvailable_ShouldChangeStatusToRented()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));

        // Act
        motorcycle.Rent();

        // Assert
        motorcycle.Status.Should().Be(MotorcycleStatus.Rented);
    }

    [Fact]
    public void RentMotorcycle_WhenNotAvailable_ShouldThrowException()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));
        motorcycle.Rent(); // Primeiro aluguel

        // Act & Assert
        var action = () => motorcycle.Rent();
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ReturnMotorcycle_WhenRented_ShouldChangeStatusToAvailable()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));
        motorcycle.Rent();

        // Act
        motorcycle.Return();

        // Assert
        motorcycle.Status.Should().Be(MotorcycleStatus.Available);
    }

    [Fact]
    public void ReturnMotorcycle_WhenNotRented_ShouldThrowException()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));

        // Act & Assert
        var action = () => motorcycle.Return();
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetOutOfService_WhenActive_ShouldChangeStatusToOutOfService()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));

        // Act
        motorcycle.SetOutOfService();

        // Assert
        motorcycle.Status.Should().Be(MotorcycleStatus.OutOfService);
    }

    [Fact]
    public void UpdateLicensePlate_WithValidPlate_ShouldUpdateSuccessfully()
    {
        // Arrange
        var motorcycle = new Motorcycle("Honda", "CB 600F", 2023, "ABC1234", "Vermelha", 600, new Money(150.00m, "BRL"));
        var newPlate = "XYZ9876";

        // Act
        motorcycle.UpdateLicensePlate(newPlate);

        // Assert
        motorcycle.LicensePlate.Should().Be(newPlate);
    }
}
