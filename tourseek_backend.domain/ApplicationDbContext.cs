using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using tourseek_backend.domain.Entities;

namespace tourseek_backend.domain
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbContextOptions<ApplicationDbContext> DbOptions { get; }

        #region DB Sets
        public new DbSet<ApplicationRole> Roles { get; set; }
        public new DbSet<ApplicationUserRole> UserRoles { get; set; }
        #endregion

        #region Constructor

        public ApplicationDbContext([NotNull] DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            DbOptions = options as DbContextOptions<ApplicationDbContext>;
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("postgis");  // PostGIS is required to be installed

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }

        #endregion
    }
}