using Microsoft.EntityFrameworkCore;
using MotoTrust.Domain.Common;
using MotoTrust.Domain.Interfaces;
using MotoTrust.Infrastructure.Data;
using System.Linq.Expressions;

namespace MotoTrust.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : EntityBase
{
    protected readonly MotoTrustDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(MotoTrustDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual void Update(T entity)
    {
        entity.Update();
        _dbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        entity.Deactivate();
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}
