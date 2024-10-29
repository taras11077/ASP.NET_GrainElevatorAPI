using System.Linq.Expressions;
using GrainElevatorAPI.Core.Interfaces;

namespace GrainElevator.Storage;

public class Repository : IRepository
{
    private readonly GrainElevatorApiContext _context;

    public Repository(GrainElevatorApiContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        var entityFromFb = _context.Set<T>().Add(entity);
        return await Task.FromResult(entityFromFb.Entity);
    }

    public async Task<T> UpdateAsync<T>(T entity) where T : class
    {
        var updated = _context.Update(entity);
        return await Task.FromResult(updated.Entity);
    }

    public async Task DeleteAsync<T>(int id) where T : class
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
            _context.Set<T>().Remove(entity);
        else
            throw new ArgumentException("Entity not found");
        
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : class
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public IQueryable<T> GetAll<T>() where T : class
    {
        return _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
    {
        return _context.Set<T>().Where(func);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}