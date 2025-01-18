using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.User;

internal class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginEntity> builder)
    {
        builder.HasKey(s => new {s.ProviderKey,s.LoginProvider });

        builder.Property(s => s.LoginProvider).HasMaxLength(500);
        builder.Property(s => s.ProviderKey).HasMaxLength(500);
        builder.Property(s => s.ProviderDisplayName).HasMaxLength(100);

        builder.ToTable("UserLogins","user");
    }
}
