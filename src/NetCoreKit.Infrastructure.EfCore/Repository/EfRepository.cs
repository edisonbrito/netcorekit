using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreKit.Domain;

namespace NetCoreKit.Infrastructure.EfCore.Repository
{
    public class EfQueryRepositoryFactory : IQueryRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EfQueryRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : IAggregateRoot
        {
            return _serviceProvider.GetService(typeof(IEfQueryRepository<TEntity>)) as IEfQueryRepository<TEntity>;
        }
    }

    public class EfRepositoryAsync<TEntity> : EfRepositoryAsync<DbContext, TEntity>, IEfRepositoryAsync<TEntity>
        where TEntity : class, IAggregateRoot
    {
        public EfRepositoryAsync(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EfQueryRepository<TEntity> : EfQueryRepository<DbContext, TEntity>, IEfQueryRepository<TEntity>
        where TEntity : class, IAggregateRoot
    {
        public EfQueryRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EfRepositoryAsync<TDbContext, TEntity> : IEfRepositoryAsync<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EfRepositoryAsync(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var entry = _dbSet.Remove(entity);
            return await Task.FromResult(entry.Entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            return await Task.FromResult(entry.Entity);
        }
    }

    public class EfQueryRepository<TDbContext, TEntity> : IEfQueryRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot
    {
        private readonly TDbContext _dbContext;

        public EfQueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
