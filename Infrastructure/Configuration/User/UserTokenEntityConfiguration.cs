using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserTokenEntity>
{
    public void Configure(EntityTypeBuilder<UserTokenEntity> builder)
    {
        builder.Property(s => s.Value);
        builder.Property(s => s.LoginProvider).HasMaxLength(450);
        builder.Property(s => s.Name).HasMaxLength(450);

        builder.HasKey(s => new {s.UserId,s.Name,s.LoginProvider });

        builder.ToTable("UserTokens","user");
    }
}
