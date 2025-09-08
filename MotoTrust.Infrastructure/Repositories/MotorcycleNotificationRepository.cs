using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Entities;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;

namespace MotoTrust.Infrastructure.Repositories;

public class MotorcycleNotificationRepository : BaseRepository<MotorcycleNotification>, IMotorcycleNotificationRepository
{
    public MotorcycleNotificationRepository(MotoTrustDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MotorcycleNotification>> GetByYearAsync(int year)
    {
        return await _context.MotorcycleNotifications
            .Where(n => n.Ano == year && n.IsActive)
            .OrderByDescending(n => n.DataEvento)
            .ToListAsync();
    }

    public async Task<IEnumerable<MotorcycleNotification>> GetByTypeAsync(string tipoNotificacao)
    {
        return await _context.MotorcycleNotifications
            .Where(n => n.TipoNotificacao == tipoNotificacao && n.IsActive)
            .OrderByDescending(n => n.DataEvento)
            .ToListAsync();
    }

    public async Task<IEnumerable<MotorcycleNotification>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.MotorcycleNotifications
            .Where(n => n.DataEvento >= startDate && n.DataEvento <= endDate && n.IsActive)
            .OrderByDescending(n => n.DataEvento)
            .ToListAsync();
    }
}
