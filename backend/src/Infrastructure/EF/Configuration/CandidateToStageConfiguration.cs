using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class CandidateToStageConfiguration : IEntityTypeConfiguration<CandidateToStage>
    {
        public void Configure(EntityTypeBuilder<CandidateToStage> builder)
        {
            builder.HasOne(cts => cts.Candidate)
                .WithMany(c => c.CandidateToStages)
                .HasForeignKey(cts => cts.CandidateId)
                .HasConstraintName("candidate_to_stage_candidate_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cts => cts.Stage)
                .WithMany(s => s.CandidateToStages)
                .HasForeignKey(cts => cts.StageId)
                .HasConstraintName("candidate_to_stage_stage_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
