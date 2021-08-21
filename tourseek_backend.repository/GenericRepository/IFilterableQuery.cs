using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace tourseek_backend.repository.GenericRepository
{
    public interface IFilterableQuery<TEntity, in TFilter>
        where TEntity : class 
        where TFilter : class
    {
        IQueryable<TEntity> AddInitialFilters(DbSet<TEntity> entities, TFilter filter, IQueryable<TEntity> queryable)
        {
            return queryable;
        }

        IQueryable<TEntity> AddQueryFilters(DbSet<TEntity> entities, TFilter filter, IQueryable<TEntity> queryable)
        {
            return queryable;
        }
    }
}