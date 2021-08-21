using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.repository.GenericRepository;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.util.Extensions;

namespace tourseek_backend.services
{
    public abstract class BaseService<TSource, TDestination, TFilter> : IQuery<TSource, TDestination, TFilter> 
        where TSource : class 
        where TDestination : class 
        where TFilter : class
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract IQueryable<TDestination> QuerySelector(DbSet<TSource> entities, IQueryable<TSource> queryable);

        public PagedList<dynamic> GetPagedList(
            ICollection<string> columns,
            TFilter filter = null,
            PaginationFilter paginationFilter = null
        )
        {
            var pagedList = UnitOfWork.Repository<TSource>().GetPagedList(this, filter, paginationFilter);
            var queryablePagedData = pagedList.Data.AsQueryable();

            if (columns == null || columns.Count == 0)
                columns = queryablePagedData.GetColumnNames().ToArray();

            return new PagedList<dynamic>
            {
                Data = queryablePagedData.DynamicSelect(columns).ToList(),
                PageLength = pagedList.PageLength,
                TotalLength = pagedList.TotalLength
            };
        }
    }
}