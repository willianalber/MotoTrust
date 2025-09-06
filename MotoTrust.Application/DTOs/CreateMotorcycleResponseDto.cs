namespace MotoTrust.Application.DTOs;

public class CreateMotorcycleResponseDto
{
    public Guid Id { get; set; }
    public string Identificador { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
