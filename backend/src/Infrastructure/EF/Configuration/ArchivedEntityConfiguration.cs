using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class ArchivedEntityConfiguration : IEntityTypeConfiguration<ArchivedEntity>
    {
        public void Configure(EntityTypeBuilder<ArchivedEntity> builder)
        {
            builder.HasIndex(p => new { p.EntityType, p.EntityId });
            builder.Property(p => p.ExpirationDate)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
