namespace MotoTrust.Application.DTOs;

public class UpdateReturnDateResponseDto
{
    public string Mensagem { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal ValorBase { get; set; }
    public decimal ValorMulta { get; set; }
    public int DiasUtilizados { get; set; }
    public int DiasAtraso { get; set; }
    public int DiasAntecipacao { get; set; }
    public string TipoCalculo { get; set; } = string.Empty;
}
