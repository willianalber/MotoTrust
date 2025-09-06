using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;

namespace MotoTrust.Infrastructure.Repositories;

public class MotorcycleRepository : BaseRepository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(MotoTrustDbContext context) : base(context)
    {
    }

    public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _dbSet.FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync()
    {
        return await _dbSet.Where(m => m.Status == MotorcycleStatus.Available && m.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByStatusAsync(MotorcycleStatus status)
    {
        return await _dbSet.Where(m => m.Status == status && m.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByPlateFilterAsync(string plateFilter)
    {
        return await _dbSet
            .Where(m => m.LicensePlate.Contains(plateFilter) && m.IsActive)
            .ToListAsync();
    }
}
