using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class StageConfiguration : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            builder.HasOne(s => s.Vacancy)
                .WithMany(v => v.Stages)
                .HasForeignKey(s => s.VacancyId)
                .HasConstraintName("stage_vacancy_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}