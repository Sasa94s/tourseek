using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.repository.GenericRepository;

namespace tourseek_backend.repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool BulkActions(Func<DbContext, bool> commitCallback);
        bool BulkActions(
            Func<DbContext, bool> actionsCallback,
            int count,
            int commitCount,
            bool recreateContext
        );
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Save();
    }
}
