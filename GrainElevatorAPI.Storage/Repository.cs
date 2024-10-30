using System.Linq.Expressions;
using GrainElevatorAPI.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GrainElevator.Storage;

public class Repository : IRepository
{
    private readonly GrainElevatorApiContext _context;
    private IDbContextTransaction _currentTransaction;

    public Repository(GrainElevatorApiContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        var entityFromDb = await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entityFromDb.Entity;
    }

    public async Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        var updated = _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return updated.Entity;
    }

    public async Task DeleteAsync<T>(int id, CancellationToken cancellationToken) where T : class
    {
        var entity = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException("Entity not found");
        }
    }

    public async Task<T> GetByIdAsync<T>(int id, CancellationToken cancellationToken) where T : class
    {
        return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public IQueryable<T> GetAll<T>() where T : class
    {
        return _context.Set<T>();
    }

    public IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> func) where T : class
    {
        return _context.Set<T>().Where(func);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    
    // методи керування транзакціями
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("Поточна транзакція вже активна");
        }
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("Немає активної транзакції для фіксації");
        }
        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("Немає активної транзакції для відкату");
        }
        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }
}