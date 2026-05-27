using Audiora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audiora.Infrastructure.Data.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(150);
        builder.Property(a => a.Bio).HasMaxLength(1000);
        builder.Property(a => a.ProfileImageUrl).HasMaxLength(500);
        builder.Property(a => a.Country).HasMaxLength(50);
        builder.Property(a => a.Genre).HasMaxLength(50);
        builder.Property(a => a.TotalRevenue).HasPrecision(18, 4);

        builder.HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<Artist>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}