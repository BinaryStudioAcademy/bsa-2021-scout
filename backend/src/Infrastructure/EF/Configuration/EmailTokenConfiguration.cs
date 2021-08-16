using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.Configuration
{
    public class EmailTokenConfiguration : IEntityTypeConfiguration<EmailToken>
    {
        public void Configure(EntityTypeBuilder<EmailToken> builder)
        {
            builder.HasOne(t => t.User)
                .WithOne(u => u.EmailToken)
                .HasConstraintName("email_token__user_FK")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
