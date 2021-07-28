using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class PoolToApplicantConfiguration : IEntityTypeConfiguration<PoolToApplicant>
    {
        public void Configure(EntityTypeBuilder<PoolToApplicant> builder)
        {
            builder.HasOne(pa => pa.Applicant)
                .WithMany(a => a.ApplicantPools)
                .HasForeignKey(pa => pa.ApplicantId)
                .HasConstraintName("pool_applicant__applicant_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.Pool)
                .WithMany(p => p.PoolApplicants)
                .HasForeignKey(pa => pa.PoolId)
                .HasConstraintName("pool_applicant__pool_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}