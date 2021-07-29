using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class PoolConfiguration : IEntityTypeConfiguration<Pool>
    {
        public void Configure(EntityTypeBuilder<Pool> builder)
        {
            builder.HasOne(p => p.Company)
                .WithMany(c => c.Pools)
                .HasForeignKey(p => p.CompanyId)
                .HasConstraintName("pool_company_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}