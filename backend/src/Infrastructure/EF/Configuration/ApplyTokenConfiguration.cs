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
    public class ApplyTokenConfiguration : IEntityTypeConfiguration<ApplyToken>
    {
        public void Configure(EntityTypeBuilder<ApplyToken> builder)
        {
            builder.HasOne(t => t.Vacancy)
                .WithMany(u => u.ApplyTokens)
                .HasForeignKey(t => t.VacancyId)
                .HasConstraintName("apply_token__vacancy_FK")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
