using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Acme.Business.Managers
{
    public abstract class BaseManager<TEntity>
        where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        protected IRepository<TEntity> Repository;

        public BaseManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Repository = _unitOfWork.GetRepository<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await Repository.AddAsync(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            Repository.Update(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            Repository.Delete(entity);
        }

        public IEnumerable<TEntity> Get()
        {
            return Repository.Get();
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true)
        {
            return Repository.Get(predicate, includeBaseGraph, null);
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includes)
        {
            return Repository.Get(predicate, includeBaseGraph, includes);
        }

        public TEntity GetSingle(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true)
        {
            return Repository.GetSingle(predicate, includeBaseGraph);
        }

        public TEntity GetSingle(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includes)
        {
            return Repository.GetSingle(predicate, includeBaseGraph, includes);
        }
    }
}