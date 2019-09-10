using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Acme.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Set of graph items that will be included on Get retrievals.
        /// </summary>
        public Expression<Func<TEntity, object>>[] DefaultGraphItems { get; protected set; }

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<TEntity>();
        }

        /// <summary>
        /// Add a new entity to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// Add a new entity to the database asynchronously
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Delete an existing entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            _unitOfWork.Context.ChangeState(entity, EntityState.Deleted);
        }

        public IEnumerable<TEntity> Get()
        {
            return Query(null, true, DefaultGraphItems);
        }

        /// <summary>
        /// Performs a query to get the first item matching the criteria.  Base graph objects are included.
        /// <remarks>
        /// Be sure to set the DefaultGraphItems property prior to using this method.
        /// </remarks>
        /// <param name="predicate"></param>
        /// <param name="includeBaseGraph"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true)
        {
            return Query(predicate, includeBaseGraph).FirstOrDefault();
        }

        /// <summary>
        /// Performs a query to get the first item matching the criteria.  Base graph objects are included.
        /// <remarks>
        /// Be sure to set the DefaultGraphItems property prior to using this method.
        /// </remarks>
        /// <param name="predicate"></param>
        /// <param name="includeBaseGraph"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph = true, params Expression<Func<TEntity, object>>[] includes)
        {
            return Query(predicate, includeBaseGraph, includes).FirstOrDefault();
        }

        /// <summary>
        /// Performs a query to get the first item matching the criteria.  Options are provided to specify graph objects to be included.
        /// <remarks>
        /// The base Repository class does not include any graph items. The DefaultGraphItems property needs to be set to accordingly.
        /// </remarks>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IncludeBaseGraph"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph, params Expression<Func<TEntity, object>>[] includes)
        {
            return Query(predicate, includeBaseGraph, includes);
        }

        /// <summary>
        /// Performs a refresh of a given sets of entities in the context 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> ReloadAsync(TEntity entity)
        {
            var entry = _unitOfWork.Context.GetEntry(entity);
            await entry.ReloadAsync();

            return entry.Entity as TEntity;
        }

        /// <summary>
        /// Performs a refresh of a given sets of entities in the context 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<TEntity> ReloadAsync(params object[] keys)
        {
            var entity = await _dbSet.FindAsync(keys);
            if (entity == null)
                return null;

            return await ReloadAsync(entity);
        }

        /// <summary>
        /// Performs an Update of a given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity)
        {
            var entry = _unitOfWork.Context.GetEntry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }

            return entity;
        }

        /// <summary>
        /// Used to build a query with the specified "where" clause and "includes" graph items.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool includeBaseGraph, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query;

            if (predicate != null)
                query = _dbSet.Where(predicate);
            else
                query = _dbSet;

            if (includeBaseGraph)
                query = QueryIncluding(query, DefaultGraphItems);

            return QueryIncluding(query, includes);
        }

        /// <summary>
        /// Used to add graph items onto a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> QueryIncluding(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}