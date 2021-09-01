using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tourseek_backend.domain.Entities;

namespace tourseek_backend.domain.Configurations
{
    public class LocationConfiguration : BaseEntityTypeConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            base.Configure(builder);
            builder.Property(s => s.Point)
                .HasColumnType("geography (point)");
        }
    }
}