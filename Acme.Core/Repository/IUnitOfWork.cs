using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Acme.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IContext Context { get; }
        Task<int> SaveAsync();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}