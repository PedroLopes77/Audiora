using Audiora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audiora.Infrastructure.Data.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("Playlists");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.CoverImageUrl)
            .HasMaxLength(500);
    }
}

public class PlaylistMusicConfiguration : IEntityTypeConfiguration<PlaylistMusic>
{
    public void Configure(EntityTypeBuilder<PlaylistMusic> builder)
    {
        builder.ToTable("PlaylistMusics");

        // Chave composta
        builder.HasKey(pm => new { pm.PlaylistId, pm.MusicId });

        builder.HasOne(pm => pm.Playlist)
            .WithMany(p => p.PlaylistMusics)
            .HasForeignKey(pm => pm.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pm => pm.Music)
            .WithMany(m => m.PlaylistMusics)
            .HasForeignKey(pm => pm.MusicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}