using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Acme.Core.Repository
{
    public interface IRepository<TEntity>
    {
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true);
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph, params Expression<Func<TEntity, object>>[] includes);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity> ReloadAsync(TEntity entity);
        Task<TEntity> ReloadAsync(params object[] keys);
    }
}