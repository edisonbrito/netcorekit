using Microsoft.EntityFrameworkCore;
using NetCoreKit.Domain;

namespace NetCoreKit.Infrastructure.EfCore.Repository
{
    public interface IEfRepositoryAsync<TEntity> : IEfRepositoryAsync<DbContext, TEntity>
        where TEntity : IAggregateRoot
    {
    }

    public interface IEfQueryRepository<out TEntity> : IEfQueryRepository<DbContext, TEntity>
        where TEntity : IAggregateRoot
    {
    }

    public interface IEfRepositoryAsync<TDbContext, TEntity> : IRepositoryAsync<TEntity>
        where TDbContext : DbContext
        where TEntity : IAggregateRoot
    {
    }

    public interface IEfQueryRepository<TDbContext, out TEntity> : IQueryRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : IAggregateRoot
    {
    }
}
