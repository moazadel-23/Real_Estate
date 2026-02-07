using System.Linq.Expressions;

namespace Real_Estate.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync
            (
            Expression<Func<TEntity, bool>>? expression = default,
            Expression<Func<TEntity, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default
            );
        Task<TEntity> GetOneAsync
            (
            Expression<Func<TEntity, bool>>? expression = default,
            Expression<Func<TEntity, object>>[]? include = null,
            bool tracking = true,
            CancellationToken cancellationToken = default
            );
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(TEntity entity, CancellationToken cancellationToken = default);
        Task CommitChange(CancellationToken cancellationToken = default);
    }
}
