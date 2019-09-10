using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Acme.Core.Repository
{
    public interface IContext : IDisposable
    {
        void ChangeState<TEntity>(TEntity Entity, EntityState state) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveAsync();
    }

}