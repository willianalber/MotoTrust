using MotoTrust.Domain.Entities;

namespace MotoTrust.Domain.Interfaces;

public interface IMotorcycleNotificationRepository : IRepository<MotorcycleNotification>
{
    Task<IEnumerable<MotorcycleNotification>> GetByYearAsync(int year);
    Task<IEnumerable<MotorcycleNotification>> GetByTypeAsync(string tipoNotificacao);
    Task<IEnumerable<MotorcycleNotification>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
