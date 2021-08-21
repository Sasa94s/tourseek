using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;

namespace tourseek_backend.repository.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllBind();
        Task<ICollection<TEntity>> GetAllBindAsync();
        Task<Dictionary<TKey, TEntity>> GetAllBindAsync<TKey>(Func<TEntity, TKey> keySelector);
        TEntity GetById(params object[] id);
        Task<TEntity> GetByIdAsync(params object[] id);
        bool Add(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        bool Remove(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
        bool RemoveById(params object[] id);
        bool BulkRemove(IEnumerable<TEntity> entities);
        Task<bool> RemoveByIdAsync(params object[] id);
        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        IQueryable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");
        public PagedList<TMappedEntity> GetPagedList<TMappedEntity, TFilter>(
            IQuery<TEntity, TMappedEntity, TFilter> queryContext,
            TFilter filter = null,
            PaginationFilter paginationFilter = null
        ) where TFilter : class
            where TMappedEntity : class;
        PagedList<dynamic> GetDynamicPagedList<TFilter>(
            IFilterableQuery<TEntity, TFilter> queryContext,
            ICollection<string> columns,
            TFilter filter = null,
            PaginationFilter paginationFilter = null
        ) where TFilter : class;
        Task<UpdateResult<TResult, TProxyDto>> UpdateBulkAsync<TProxyDto, TFilter, TResult>(
            IUpdatableQuery<TProxyDto, TEntity, TFilter, TResult> queryContext,
            TFilter filter,
            Action<TEntity> action,
            bool singleUpdateFlag = false
        ) 
            where TProxyDto : class
            where TFilter : class;

        IEnumerable<TEntity> GetResultByFilter(Expression<Func<TEntity, bool>> filter);
        int GetCountByFilter(Expression<Func<TEntity, bool>> filter);
        TResult GetResultByFilter<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector
        );
        void BulkInsert(IEnumerable<TEntity> entities, int commitCount, bool recreateContext);
        bool BulkInsert(IEnumerable<TEntity> entities);
        void BulkInsert(IEnumerable<TEntity> entities, bool recreateContext);
        bool BulkUpdate(IEnumerable<TEntity> entities);
    }
}
