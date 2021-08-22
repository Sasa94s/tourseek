using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.domain;
using tourseek_backend.domain.Core;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.util.Extensions;

namespace tourseek_backend.repository.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private ApplicationDbContext _context;
        private readonly DbSet<TEntity> _set;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public DbSet<TEntity> Set => _set;
        public ApplicationDbContext Context => _context;


        public bool Add(TEntity entity)
        {
            Set.Add(entity);

            return Context.SaveChanges() > 0;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await Set.AddAsync(entity);

            return await Context.SaveChangesAsync() > 0;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Set;
        }

        //public async Task<IQueryable<TEntity>> GetAllAsync()
        //{
        //    return await Set;
        //}
        public IEnumerable<TEntity> GetAllBind()
        {
            return Set.ToList();
        }

        public async Task<ICollection<TEntity>> GetAllBindAsync()
        {
            return await Set.AsQueryable().ToListAsync();
        }

        public async Task<Dictionary<TKey, TEntity>> GetAllBindAsync<TKey>(Func<TEntity, TKey> keySelector)
        {
            return await Set.AsQueryable().ToDictionaryAsync(keySelector);
        }

        public TEntity GetById(params object[] id)
        {
            return Set.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(params object[] id)
        {
            return await Set.FindAsync(id);
        }

        public bool Remove(TEntity entity)
        {
            Set.Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;

            return Context.SaveChanges() > 0;
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            Set.Attach(entity);
            _context.Entry<TEntity>(entity).State = EntityState.Deleted;

            return (await Context.SaveChangesAsync()) > 0;
        }

        public bool RemoveById(params object[] id)
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Remove(entity);
            return Context.SaveChanges() > 0;
        }

        public async Task<bool> RemoveByIdAsync(params object[] id)
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Remove(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public bool Update(TEntity entity)
        {
            Set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return Context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            Set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            Set.Update(entity);

            return (await Context.SaveChangesAsync()) > 0;
        }

        public int GetCountByFilter(Expression<Func<TEntity, bool>> filter)
        {
            return _set.Count(filter);
        }

        public TResult GetResultByFilter<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
        {
            return _set
                .Where(filter)
                .Select(selector)
                .SingleOrDefault();
        }

        public IEnumerable<TEntity> GetResultByFilter(Expression<Func<TEntity, bool>> filter)
        {
            return _set
                .Where(filter)
                .ToList();
        }

        public IQueryable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public TResult GetMax<TResult>(Expression<Func<TEntity, TResult>> filter)
        {
            return _set.Max(filter);
        }

        public PagedList<TEntity> GetPagedList<TFilter>(
            IFilterableQuery<TEntity, TFilter> queryContext,
            TFilter filter = null,
            PaginationFilter paginationFilter = null) where TFilter : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            queryable = queryContext.AddInitialFilters(_set, filter, queryable);

            var totalRows = queryable.Count();

            if (paginationFilter == null)
            {
                return new PagedList<TEntity>()
                {
                    Data = queryable.AsEnumerable(),
                    PageLength = totalRows,
                    TotalLength = totalRows,
                };
            }

            queryable = queryContext.AddQueryFilters(_set, filter, queryable);

            var filteredRows = queryable.Count();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            if (paginationFilter.Paging)
            {
                queryable = queryable.Skip(skip).Take(paginationFilter.PageSize);
            }

            return new PagedList<TEntity>()
            {
                Data = queryable,
                PageLength = filteredRows,
                TotalLength = totalRows,
            };
        }

        public PagedList<TMappedEntity> GetPagedList<TMappedEntity, TFilter>(
            IQuery<TEntity, TMappedEntity, TFilter> queryContext,
            TFilter filter = null,
            PaginationFilter paginationFilter = null
        ) where TFilter : class
            where TMappedEntity : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            queryable = queryContext.AddInitialFilters(_set, filter, queryable);

            var totalRows = queryable.Count();

            if (paginationFilter == null)
            {
                return new PagedList<TMappedEntity>
                {
                    Data = queryContext.QuerySelector(_set, queryable).ToList(),
                    PageLength = totalRows,
                    TotalLength = totalRows,
                };
            }

            queryable = queryContext.AddQueryFilters(_set, filter, queryable);
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var mappedQueryable = queryContext.QuerySelector(_set, queryable);
            mappedQueryable = queryContext.AddFinalFilters(_set, filter, mappedQueryable);
            var filteredRows = mappedQueryable.Count();

            if (paginationFilter.Paging)
            {
                mappedQueryable = mappedQueryable.Skip(skip).Take(paginationFilter.PageSize);
            }

            return new PagedList<TMappedEntity>
            {
                Data = mappedQueryable.ToList(),
                PageLength = filteredRows,
                TotalLength = totalRows,
            };
        }

        public PagedList<dynamic> GetDynamicPagedList<TFilter>(
            IFilterableQuery<TEntity, TFilter> queryContext,
            ICollection<string> columns,
            TFilter filter = null,
            PaginationFilter paginationFilter = null) where TFilter : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            queryable = queryContext.AddInitialFilters(_set, filter, queryable);

            if (columns == null || columns.Count == 0)
            {
                columns = queryable.GetColumnNames().ToArray();
            }

            var totalRows = queryable.Count();

            if (paginationFilter == null)
            {
                return new PagedList<dynamic>()
                {
                    Data = queryable.AsEnumerable(),
                    PageLength = totalRows,
                    TotalLength = totalRows,
                };
            }

            queryable = queryContext.AddQueryFilters(_set, filter, queryable);
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var filteredRows = queryable.Count();

            if (paginationFilter.Paging)
            {
                queryable = queryable.Skip(skip).Take(paginationFilter.PageSize);
            }

            return new PagedList<dynamic>()
            {
                Data = queryable.DynamicSelect(columns),
                PageLength = filteredRows,
                TotalLength = totalRows,
            };
        }

        public async Task<UpdateResult<TResult, TProxyDto>> UpdateBulkAsync<TProxyDto, TFilter, TResult>(
            IUpdatableQuery<TProxyDto, TEntity, TFilter, TResult> queryContext,
            TFilter filter,
            Action<TEntity> action,
            bool singleUpdateFlag = false
        )
            where TProxyDto : class
            where TFilter : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            queryable = queryContext.AddInitialFilters(_set, filter, queryable);

            queryable = queryContext.AddQueryFilters(_set, filter, queryable);

            if (!await queryable.AnyAsync())
                return new UpdateResult<TResult, TProxyDto>
                {
                    AffectedCount = -1,
                    EntitiesValuesList = Enumerable.Empty<TResult>().ToImmutableList(),
                };

            if (singleUpdateFlag && queryable.Count() != 1)
                return new UpdateResult<TResult, TProxyDto>
                {
                    AffectedCount = -2,
                    EntitiesValuesList = Enumerable.Empty<TResult>().ToImmutableList(),
                };
            var entities = await queryable.ToListAsync();
            foreach (var entity in entities)
            {
                action(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            // await queryable.ForEachAsync(action);

            return new UpdateResult<TResult, TProxyDto>
            {
                AffectedCount = await Context.SaveChangesAsync().ConfigureAwait(false),
                EntitiesValuesList = entities.Select(queryContext.BeforeUpdateSelector).ToImmutableList(),
            };
        }

        public void BulkInsert(IEnumerable<TEntity> entities, int commitCount, bool recreateContext)
        {
            int count = 0;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            foreach (var entity in entities)
            {
                Add(entity);
                count++;

                if (count % commitCount != 0) continue;

                // save changes <after adding each 100 entity>
                _context.SaveChanges();
            }

            // save changes for the remainder of 100 entity
            _context.SaveChanges();
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public void BulkInsert(IEnumerable<TEntity> entities, bool recreateContext)
        {
            BulkInsert(entities, 100, recreateContext);
        }

        public bool BulkInsert(IEnumerable<TEntity> entities)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.Set<TEntity>().AddRange(entities);
            var status = _context.SaveChanges() > 0;
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return status;
        }


        public bool BulkRemove(IEnumerable<TEntity> entities)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.Set<TEntity>().RemoveRange(entities);
            var status = _context.SaveChanges() > 0;

            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return status;
        }
        public bool BulkUpdate(IEnumerable<TEntity> entities)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            foreach (var entity in entities)
            {
                Set.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            var status = _context.SaveChanges() > 0;
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return status;
        }
    }
}
