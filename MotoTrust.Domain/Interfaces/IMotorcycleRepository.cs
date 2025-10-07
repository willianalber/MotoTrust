using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;

namespace MotoTrust.Domain.Interfaces;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate);
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByStatusAsync(MotorcycleStatus status);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByPlateFilterAsync(string plateFilter);
}
