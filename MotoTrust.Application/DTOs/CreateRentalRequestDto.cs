namespace MotoTrust.Application.DTOs;

public class CreateRentalRequestDto
{
    public Guid EntregadorId { get; set; }
    public Guid MotoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public DateTime DataPrevisaoTermino { get; set; }
    public int Plano { get; set; }
}
