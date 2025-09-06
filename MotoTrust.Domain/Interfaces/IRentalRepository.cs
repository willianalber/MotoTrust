using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Domain.Interfaces;

public interface IRentalRepository : IRepository<Rental>
{
    Task<IEnumerable<Rental>> GetRentalsByEntregadorIdAsync(string entregadorId);
    Task<IEnumerable<Rental>> GetRentalsByMotoIdAsync(string motoId);
    Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status);
    Task<IEnumerable<Rental>> GetActiveRentalsAsync();
}
