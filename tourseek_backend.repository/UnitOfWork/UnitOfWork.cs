using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.domain;
using tourseek_backend.repository.GenericRepository;

namespace tourseek_backend.repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool BulkActions(Func<DbContext, bool> actionsCallback)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!actionsCallback(_context))
                {
                    return false;
                }

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
            }

            return false;
        }

        public bool BulkActions(
            Func<DbContext, bool> actionsCallback,
            int count,
            int commitCount,
            bool recreateContext
        )
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            if (!actionsCallback(_context)) return false;

            if (count % commitCount != 0) return true;

            _context.SaveChanges();

            return true;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public bool BulkActions(Func<ApplicationDbContext, bool> commitCallback)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!commitCallback(_context))
                {
                    return false;
                }

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
            }

            return false;
        }
    }
}
