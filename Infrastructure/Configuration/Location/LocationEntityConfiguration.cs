using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Location;

internal class LocationEntityConfiguration : IEntityTypeConfiguration<LocationEntity>
{
    public void Configure(EntityTypeBuilder<LocationEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.ToTable("Locations", "ad");
        builder.HasIndex(c => c.Name);

        builder.Property(c => c.Name).HasMaxLength(100);

        builder.HasMany(c => c.Ads)
            .WithOne(c => c.Location)
            .HasForeignKey(c => c.LocationId) ;
    }
}
