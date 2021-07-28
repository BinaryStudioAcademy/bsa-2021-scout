using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(_ => _.DomainEvents);
            
            builder.HasOne(u => u.Vacancy)
                .WithOne(v => v.ResponsibleHr)
                .HasForeignKey<Vacancy>(v => v.ResponsibleHrId)
                .HasConstraintName("vacancy_user_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
