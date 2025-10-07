namespace MotoTrust.Application.DTOs;

public class CreateDeliveryPersonRequestDto
{
    public string Identificador { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string NumeroCNH { get; set; } = string.Empty;
    public string TipoCNH { get; set; } = string.Empty;
    public string? ImagemCNH { get; set; }
}
