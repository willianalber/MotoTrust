using FluentAssertions;
using MotoTrust.Domain.ValueObjects;

namespace MotoTrust.Tests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void CreateMoney_WithValidAmountAndCurrency_ShouldCreateSuccessfully()
    {
        // Arrange
        var amount = 150.50m;
        var currency = "BRL";

        // Act
        var money = new Money(amount, currency);

        // Assert
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void CreateMoney_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange
        var amount = -100.00m;
        var currency = "BRL";

        // Act & Assert
        var action = () => new Money(amount, currency);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Amount cannot be negative*");
    }

    [Fact]
    public void CreateMoney_WithEmptyCurrency_ShouldThrowException()
    {
        // Arrange
        var amount = 100.00m;
        var currency = "";

        // Act & Assert
        var action = () => new Money(amount, currency);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Currency cannot be empty*");
    }

    [Fact]
    public void AddMoney_WithSameCurrency_ShouldReturnCorrectSum()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(50.00m, "BRL");

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(150.00m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void AddMoney_WithDifferentCurrency_ShouldThrowException()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(50.00m, "USD");

        // Act & Assert
        var action = () => money1 + money2;
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add money with different currencies");
    }

    [Fact]
    public void SubtractMoney_WithSameCurrency_ShouldReturnCorrectDifference()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(30.00m, "BRL");

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(70.00m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void MultiplyMoney_WithValidMultiplier_ShouldReturnCorrectResult()
    {
        // Arrange
        var money = new Money(100.00m, "BRL");
        var multiplier = 2.5m;

        // Act
        var result = money * multiplier;

        // Assert
        result.Amount.Should().Be(250.00m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void EqualsMoney_WithSameAmountAndCurrency_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(100.00m, "BRL");

        // Act & Assert
        money1.Should().Be(money2);
    }

    [Fact]
    public void EqualsMoney_WithDifferentAmount_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money(100.00m, "BRL");
        var money2 = new Money(200.00m, "BRL");

        // Act & Assert
        money1.Should().NotBe(money2);
    }

    [Fact]
    public void ToString_WithValidMoney_ShouldReturnFormattedString()
    {
        // Arrange
        var money = new Money(150.50m, "BRL");

        // Act
        var result = money.ToString();

        // Assert
        result.Should().Contain("150,50");
        result.Should().Contain("BRL");
    }
}
