using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Configurations
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : class, IBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}