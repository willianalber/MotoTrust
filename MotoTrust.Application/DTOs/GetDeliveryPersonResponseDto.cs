using MotoTrust.Domain.Enums;

namespace MotoTrust.Application.DTOs;

public class GetDeliveryPersonResponseDto
{
    public Guid Id { get; set; }
    public string Identificador { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string NumeroCNH { get; set; } = string.Empty;
    public string TipoCNH { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}


