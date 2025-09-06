using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;

namespace MotoTrust.Infrastructure.Repositories;

public class DeliveryPersonRepository : BaseRepository<DeliveryPerson>, IDeliveryPersonRepository
{
    public DeliveryPersonRepository(MotoTrustDbContext context) : base(context)
    {
    }

    public async Task<DeliveryPerson?> GetByIdentificadorAsync(string identificador)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Identificador == identificador && e.IsActive);
    }
}
