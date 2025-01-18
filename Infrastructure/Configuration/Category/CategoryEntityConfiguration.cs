using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Category;

internal class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.ToTable("Categories", "ad");
        builder.HasIndex(c => c.Name);

        builder.Property(c => c.Name).HasMaxLength(100);

        builder.HasMany(c => c.Ads)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId);
    }
}
