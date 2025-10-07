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

    public async Task<DeliveryPerson?> GetByCNPJAsync(string cnpj)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.CNPJ == cnpj && e.IsActive);
    }

    public async Task<DeliveryPerson?> GetByCNHAsync(string numeroCNH)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.NumeroCNH == numeroCNH && e.IsActive);
    }
}
