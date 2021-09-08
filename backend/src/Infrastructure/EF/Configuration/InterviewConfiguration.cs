using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Configuration
{
    public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.HasOne(i => i.Vacancy)
                   .WithMany()
                   .HasForeignKey(i => i.VacancyId)
                   .HasConstraintName("interview__vacancy_FK")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
