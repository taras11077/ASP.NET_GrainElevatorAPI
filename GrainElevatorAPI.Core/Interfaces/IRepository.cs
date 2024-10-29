using System.Linq.Expressions;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRepository
{
    Task<T> AddAsync<T>(T entity) where T : class;
    Task<T> UpdateAsync<T>(T entity) where T : class;
    Task DeleteAsync<T>(int id) where T : class;

    Task<T> GetByIdAsync<T>(int id) where T : class;
    IQueryable<T> GetAll<T>() where T : class;
    Task<IEnumerable<T>> GetQuery<T>(Expression<Func<T, bool>> func) where T : class;

    Task SaveChangesAsync();
}