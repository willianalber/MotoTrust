using FluentAssertions;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Tests.Domain.Entities;

public class DeliveryPersonTests
{
    [Fact]
    public void CreateDeliveryPerson_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var identificador = "entregador123";
        var nome = "João da Silva";
        var cnpj = "12345678901234";
        var dataNascimento = new DateTime(1990, 1, 1);
        var numeroCNH = "12345678900";
        var tipoCNH = LicenseType.A;

        // Act
        var deliveryPerson = new DeliveryPerson(identificador, nome, cnpj, dataNascimento, numeroCNH, tipoCNH);

        // Assert
        deliveryPerson.Identificador.Should().Be(identificador);
        deliveryPerson.Nome.Should().Be(nome);
        deliveryPerson.CNPJ.Should().Be(cnpj);
        deliveryPerson.DataNascimento.Should().Be(dataNascimento);
        deliveryPerson.NumeroCNH.Should().Be(numeroCNH);
        deliveryPerson.TipoCNH.Should().Be(tipoCNH);
    }

    [Fact]
    public void UpdateCNHImage_WithValidImage_ShouldUpdateSuccessfully()
    {
        // Arrange
        var deliveryPerson = new DeliveryPerson("entregador123", "João da Silva", "12345678901234", 
            new DateTime(1990, 1, 1), "12345678900", LicenseType.A);
        var newImage = "base64encodedimage";

        // Act
        deliveryPerson.UpdateCNHImage(newImage);

        // Assert
        deliveryPerson.ImagemCNH.Should().Be(newImage);
    }

    [Fact]
    public void UpdateCNHImage_WithEmptyImage_ShouldThrowException()
    {
        // Arrange
        var deliveryPerson = new DeliveryPerson("entregador123", "João da Silva", "12345678901234", 
            new DateTime(1990, 1, 1), "12345678900", LicenseType.A);

        // Act & Assert
        var action = () => deliveryPerson.UpdateCNHImage("");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Imagem da CNH não pode ser vazia*");
    }
}
