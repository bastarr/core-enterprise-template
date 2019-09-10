using System;
using System.Threading.Tasks;

namespace Acme.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly IServiceProvider _serviceProvider;

        public IContext Context { get; private set; }

        public UnitOfWork(IContext context, IServiceProvider serviceProvider)
        {
            Context = context;
            _serviceProvider = serviceProvider;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveAsync();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IRepository<TEntity>)_serviceProvider.GetService(typeof(IRepository<TEntity>));
        }
    }
}