using Audiora.Domain.Entities;
using Audiora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audiora.Infrastructure.Data.Configurations;

public class MusicConfiguration : IEntityTypeConfiguration<Music>
{
    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.ToTable("Musics");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.AudioUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(m => m.Genre)
            .HasConversion<int>();

        builder.Property(m => m.Lyrics)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(m => m.ArtistId)
            .HasDatabaseName("IX_Musics_ArtistId");

        builder.HasIndex(m => m.Genre)
            .HasDatabaseName("IX_Musics_Genre");

        builder.HasOne(m => m.Artist)
            .WithMany(a => a.Musics)
            .HasForeignKey(m => m.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Album)
            .WithMany(a => a.Musics)
            .HasForeignKey(m => m.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}