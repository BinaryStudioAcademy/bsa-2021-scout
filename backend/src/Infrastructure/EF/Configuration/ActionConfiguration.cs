using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class ActionConfiguration : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.HasOne(a => a.Stage)
                .WithMany(s => s.Actions)
                .HasForeignKey(a => a.StageId)
                .HasConstraintName("action_stage_FK")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}