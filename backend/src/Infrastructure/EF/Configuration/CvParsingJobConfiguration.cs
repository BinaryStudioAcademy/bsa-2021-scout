using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class CvParsingJobConfiguration : IEntityTypeConfiguration<CvParsingJob>
    {
        public void Configure(EntityTypeBuilder<CvParsingJob> builder)
        {
            builder.HasOne(cpj => cpj.Trigger)
                .WithMany(u => u.CvParsingJobs)
                .HasForeignKey(cpj => cpj.TriggerId)
                .HasConstraintName("cv_parsing_job_user_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
