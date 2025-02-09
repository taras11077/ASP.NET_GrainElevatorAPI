using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRepository
{
    Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    Task DeleteAsync<T>(int id, CancellationToken cancellationToken) where T : class;
    Task<T> GetByIdAsync<T>(int id, CancellationToken cancellationToken) where T : class;
    IQueryable<T> GetAll<T>() where T : class;
    IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> func) where T : class;
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    // методи керування транзакціями
    IExecutionStrategy CreateExecutionStrategy();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    //Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}