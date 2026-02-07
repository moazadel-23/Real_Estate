using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Real_Estate.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ILogger<Repository<TEntity>> _logger;
        private DbSet<TEntity> _dbSet;
        private ApplicationDbContext context;

        public Repository(ILogger<Repository<TEntity>> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
            _dbSet = context.Set<TEntity>();
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

        public async Task CommitChange(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error committing changes to the database");
                throw;
            }
        }

        public void Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity of type {typeof(TEntity).Name}");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object>>[]? include = default, bool tracking = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            if(expression is not null)
                query = query.Where(expression);
            if(include is not null)
                foreach(var includitem in include)
                    query = query.Include(includitem);
            if(!tracking)
                query = query.AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object>>[]? include = default, bool tracking = true, CancellationToken cancellationToken = default)
        {
            return (await GetAllAsync(expression, include, tracking, cancellationToken)).FirstOrDefault()!;
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
