using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class ReviewToStageConfiguration : IEntityTypeConfiguration<ReviewToStage>
    {
        public void Configure(EntityTypeBuilder<ReviewToStage> builder)
        {
            builder.HasOne(rts => rts.Stage)
                .WithMany(s => s.ReviewToStages)
                .HasForeignKey(rts => rts.StageId)
                .HasConstraintName("review_to_stage_stage_FK")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rts => rts.Review)
                .WithMany(r => r.ReviewToStages)
                .HasForeignKey(rts => rts.ReviewId)
                .HasConstraintName("review_to_stage_review_FK")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
