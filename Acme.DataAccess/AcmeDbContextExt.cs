using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Acme.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Acme.DataAccess
{
    public class AcmeDbContextExt : AcmeDbContext, IContext
    {
        public AcmeDbContextExt(DbContextOptions options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true; 
        }

        public AcmeDbContextExt()
            : base(new DbContextOptionsBuilder().Options)
        {

        }

        public void ChangeState<TEntity>(TEntity entity, EntityState state) where TEntity : class
        {
            Entry<TEntity>(entity).State = state;
        }

        public virtual async Task<int> SaveAsync()
        {

            if (this.ChangeTracker.Entries().Any(IsChanged))
            {
                var count = await this.SaveChangesAsync(true);
                return count;
            }
            else
            {
                return 0;
            }
        }

        private static bool IsChanged(EntityEntry entity)
        {
            return IsStateEqual(entity, EntityState.Added) ||
                IsStateEqual(entity, EntityState.Deleted) ||
                IsStateEqual(entity, EntityState.Modified);
        }

        private static bool IsStateEqual(EntityEntry entity, EntityState state)
        {
            return (entity.State & state) == state;
        }

        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry<TEntity>(entity);
        }
    }
}