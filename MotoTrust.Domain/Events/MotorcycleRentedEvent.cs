using MotoTrust.Domain.Events;

namespace MotoTrust.Domain.Events;

public class MotorcycleRentedEvent : DomainEvent
{
    public Guid RentalId { get; }
    public string EntregadorId { get; }
    public string MotoId { get; }
    public DateTime DataInicio { get; }
    public DateTime DataPrevisaoTermino { get; }

    public MotorcycleRentedEvent(Guid rentalId, string entregadorId, string motoId, 
                               DateTime dataInicio, DateTime dataPrevisaoTermino)
    {
        RentalId = rentalId;
        EntregadorId = entregadorId;
        MotoId = motoId;
        DataInicio = dataInicio;
        DataPrevisaoTermino = dataPrevisaoTermino;
    }
}
