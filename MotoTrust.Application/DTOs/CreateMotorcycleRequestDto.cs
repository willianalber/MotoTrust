namespace MotoTrust.Application.DTOs;

public class CreateMotorcycleRequestDto
{
    public string Identificador { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int CapacidadeMotor { get; set; }
    public decimal ValorDiaria { get; set; }
}
