using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasOne(v => v.Company)
                .WithMany(c => c.Vacancies)
                .HasForeignKey(v => v.CompanyId)
                .HasConstraintName("vacancy_company_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Project)
                .WithMany(p => p.Vacancies)
                .HasForeignKey(v => v.ProjectId)
                .HasConstraintName("vacancy_project_FK")
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(v => v.CompanyId)
            //    .HasColumnType("uniqueidentifier");

            //builder.Property(v => v.ResponsibleHrId)
            //    .HasColumnType("uniqueidentifier");

            //builder.Property(v => v.ProjectId)
            //    .HasColumnType("uniqueidentifier");
        }
    }
}