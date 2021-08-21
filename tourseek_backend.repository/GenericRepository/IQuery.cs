using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace tourseek_backend.repository.GenericRepository
{
    public interface IQuery<TSource, TDestination, in TFilter> : IFilterableQuery<TSource, TFilter>
        where TSource : class
        where TDestination : class
        where TFilter : class
    {
        IQueryable<TDestination> QuerySelector(
            DbSet<TSource> entities,
            IQueryable<TSource> queryable
        );

        IQueryable<TDestination> AddFinalFilters(
            DbSet<TSource> entities,
            TFilter filter,
            IQueryable<TDestination> queryable
        )
        {
            return queryable;
        }
    }
}