using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Iris.DataLayer
{
    public static class EfExtensionMethods
    {
        public static IEnumerable<TEntity> AddRange<TEntity>(this IDbSet<TEntity> dbset, IEnumerable<TEntity> entitiesToAdd) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).AddRange(entitiesToAdd);
        }

        public static IEnumerable<TEntity> RemoveRange<TEntity>(this IDbSet<TEntity> dbset, IEnumerable<TEntity> entitiesToDelete) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).RemoveRange(entitiesToDelete);
        }

        public static Task<TEntity> FindAsync<TEntity>(this IDbSet<TEntity> dbset, CancellationToken cancellationToken, params object[] keyValues) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).FindAsync(cancellationToken, keyValues);
        }

        public static Task<TEntity> FindAsync<TEntity>(this IDbSet<TEntity> dbset, params object[] keyValues) where TEntity : class
        {
            return ((DbSet<TEntity>)dbset).FindAsync(keyValues);
        }
    }
}
