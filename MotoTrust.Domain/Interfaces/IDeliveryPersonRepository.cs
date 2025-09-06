using MotoTrust.Domain.Entities;

namespace MotoTrust.Domain.Interfaces;

public interface IDeliveryPersonRepository : IRepository<DeliveryPerson>
{
    Task<DeliveryPerson?> GetByIdentificadorAsync(string identificador);
}
