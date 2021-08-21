using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace tourseek_backend.domain.Core
{
    public class ApplicationDbContext : DbContext
    {
        public DbContextOptions<ApplicationDbContext> DbOptions { get; }

        #region DB Sets

        #endregion

        #region Constructor

        public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
        {
            DbOptions = options as DbContextOptions<ApplicationDbContext>;
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());
            modelBuilder.HasPostgresExtension("postgis");
        }

        #endregion
    }
}