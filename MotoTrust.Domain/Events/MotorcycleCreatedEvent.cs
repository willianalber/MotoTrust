namespace MotoTrust.Domain.Events;

public class MotorcycleCreatedEvent : DomainEvent
{
    public Guid MotorcycleId { get; }
    public string Identificador { get; }
    public int Ano { get; }
    public string Modelo { get; }
    public string Placa { get; }
    public DateTime CreatedAt { get; }

    public MotorcycleCreatedEvent(Guid motorcycleId, string identificador, int ano, string modelo, string placa, DateTime createdAt)
    {
        MotorcycleId = motorcycleId;
        Identificador = identificador;
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
        CreatedAt = createdAt;
    }
}
