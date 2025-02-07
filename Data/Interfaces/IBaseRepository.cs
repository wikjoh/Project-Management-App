using System.Linq.Expressions;

namespace Data.Interfaces;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> CreateAsync(TEntity entity);
    Task<bool> DeleteOneAsync(TEntity entity);
    Task<IEnumerable<TEntity>?> GetAllAsync();
    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> UpdateOneAsync(TEntity entityToUpdate, Expression<Func<TEntity, bool>> predicate);
}