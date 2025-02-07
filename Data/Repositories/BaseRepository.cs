using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();



    // CREATE
    public async Task<TEntity?> CreateAsync(TEntity entity)
    {
        if (entity != null)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating {nameof(TEntity)} entity. {ex.Message}");
                return null;
            }
        }

        return null;
    }


    // READ
    public async Task<IEnumerable<TEntity>?> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }


    public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate != null)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            return entity;
        }

        return null;
    }


    public async Task<bool?> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error check if {nameof(TEntity)} entity exists. {ex.Message}");
            return null;
        }
    }


    // UPDATE
    public async Task<TEntity?> UpdateOneAsync(TEntity entityToUpdate, Expression<Func<TEntity, bool>> predicate)
    {
        if (entityToUpdate != null && predicate != null)
        {
            try
            {
                var existingEntity = await _dbSet.FirstOrDefaultAsync(predicate) ?? null!;
                if (existingEntity != null)
                {
                    _dbSet.Entry(existingEntity).CurrentValues.SetValues(entityToUpdate);
                    await _context.SaveChangesAsync();
                    return await _dbSet.FirstOrDefaultAsync(predicate);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating {nameof(TEntity)} entity. {ex.Message}");
                return null;
            }
        }

        return null;
    }


    // DELETE
    public async Task<bool> DeleteOneAsync(TEntity entity)
    {
        if (entity != null)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting {nameof(TEntity)} entity. {ex.Message}");
                return false;
            }
        }

        return false;
    }
}
