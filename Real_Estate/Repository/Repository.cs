using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Real_Estate.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ILogger<Repository<TEntity>> _logger;
        private DbSet<TEntity> _dbSet;

        public Repository(ILogger<Repository<TEntity>> logger, DbSet<TEntity> dbSet)
        {
            _logger = logger;
            _dbSet = dbSet;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity of type {typeof(TEntity).Name}");
                throw;
            }
        }

        public Task CommitChange(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity of type {typeof(TEntity).Name} ");
                throw;
            }
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object>>[]? include = null, bool tracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object>>[]? include = null, bool tracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error updating entity of type {typeof(TEntity).Name}");
                throw;
            }
        }
    }
}
