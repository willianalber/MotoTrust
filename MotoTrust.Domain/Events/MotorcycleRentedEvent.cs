namespace MotoTrust.Domain.Events;

public class MotorcycleRentedEvent : DomainEvent
{
    public Guid RentalId { get; }
    public Guid EntregadorId { get; }
    public Guid MotoId { get; }
    public DateTime DataInicio { get; }
    public DateTime DataPrevisaoTermino { get; }

    public MotorcycleRentedEvent(Guid rentalId, Guid entregadorId, Guid motoId, DateTime dataInicio, DateTime dataPrevisaoTermino)
    {
        RentalId = rentalId;
        EntregadorId = entregadorId;
        MotoId = motoId;
        DataInicio = dataInicio;
        DataPrevisaoTermino = dataPrevisaoTermino;
    }
}
