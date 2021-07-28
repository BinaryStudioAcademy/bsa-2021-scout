using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class CompanyToUserConfiguration : IEntityTypeConfiguration<CompanyToUser>
    {
        public void Configure(EntityTypeBuilder<CompanyToUser> builder)
        {
            builder.HasOne(cu => cu.User)
                .WithMany(u => u.UserCompanies)
                .HasForeignKey(cu => cu.UserId)
                .HasConstraintName("company_user__user_FK")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cu => cu.Company)
                .WithMany(c => c.Recruiters)
                .HasForeignKey(cu => cu.CompanyId)
                .HasConstraintName("company_user__company_FK")
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(cu => cu.CompanyId)
            //    .HasColumnType("uniqueidentifier");

            //builder.Property(cu => cu.UserId)
            //    .HasColumnType("uniqueidentifier");
        }
    }
}