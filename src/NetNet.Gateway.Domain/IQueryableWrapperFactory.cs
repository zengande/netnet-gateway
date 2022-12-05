using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace NetNet.Gateway;

public interface IQueryableWrapperFactory
{
    IQueryableWrapper<TEntity> CreateWrapper<TEntity>(IQueryable<TEntity> queryable) where TEntity : class;

    IQueryableWrapper<TEntity> CreateWrapper<TEntity, TKey>(IRepository<TEntity, TKey> repository) where TEntity : class, IEntity<TKey>;
}

