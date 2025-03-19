using System.Linq.Expressions;

namespace App.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(int id);
        Task RemoveRange(IEnumerable<TEntity> entities);
    }
}
