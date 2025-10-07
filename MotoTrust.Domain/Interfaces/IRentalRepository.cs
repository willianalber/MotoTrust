using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Domain.Interfaces;

public interface IRentalRepository : IRepository<Rental>
{
    Task<IEnumerable<Rental>> GetRentalsByEntregadorIdAsync(Guid entregadorId);
    Task<IEnumerable<Rental>> GetRentalsByMotoIdAsync(Guid motoId);
    Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status);
    Task<IEnumerable<Rental>> GetActiveRentalsAsync();
}
