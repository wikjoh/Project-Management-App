using System.Linq.Expressions;

namespace Data.Interfaces;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    Task CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool?> ExistsAsync(Expression<Func<TEntity, bool>> expression);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task<int?> SaveAsync();
}