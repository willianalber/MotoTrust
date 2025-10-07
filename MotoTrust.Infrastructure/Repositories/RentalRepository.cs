using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Enums;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;

namespace MotoTrust.Infrastructure.Repositories;

public class RentalRepository : BaseRepository<Rental>, IRentalRepository
{
    public RentalRepository(MotoTrustDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Rental>> GetRentalsByEntregadorIdAsync(Guid entregadorId)
    {
        return await _dbSet
            .Where(r => r.EntregadorId == entregadorId && r.IsActive)
            .Include(r => r.Entregador)
            .Include(r => r.Moto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByMotoIdAsync(Guid motoId)
    {
        return await _dbSet
            .Where(r => r.MotoId == motoId && r.IsActive)
            .Include(r => r.Entregador)
            .Include(r => r.Moto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(RentalStatus status)
    {
        return await _dbSet
            .Where(r => r.Status == status && r.IsActive)
            .Include(r => r.Entregador)
            .Include(r => r.Moto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
    {
        return await _dbSet
            .Where(r => r.Status == RentalStatus.Active && r.IsActive)
            .Include(r => r.Entregador)
            .Include(r => r.Moto)
            .ToListAsync();
    }
}
