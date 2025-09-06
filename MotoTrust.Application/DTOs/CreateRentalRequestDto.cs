namespace MotoTrust.Application.DTOs;

public class CreateRentalRequestDto
{
    public string EntregadorId { get; set; } = string.Empty;
    public string MotoId { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public DateTime DataPrevisaoTermino { get; set; }
    public int Plano { get; set; }
}
