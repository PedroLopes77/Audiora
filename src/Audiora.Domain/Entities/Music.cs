using Audiora.Domain.Common;
using Audiora.Domain.Enums;

namespace Audiora.Domain.Entities;

public class Music : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public int DurationSeconds { get; set; }
    public MusicGenre Genre { get; set; }
    public string? Lyrics { get; set; }
    public long StreamCount { get; set; } = 0;
    public bool IsExplicit { get; set; } = false;
    public bool AllowDownload { get; set; } = true;
    public int ReleaseYear { get; set; }

    // Foreign Keys
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;

    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }

    // Navigation properties
    public ICollection<PlaylistMusic> PlaylistMusics { get; set; } = new List<PlaylistMusic>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<History> Histories { get; set; } = new List<History>();

    public string GetFormattedDuration()
    {
        var ts = TimeSpan.FromSeconds(DurationSeconds);
        return ts.Hours > 0
            ? $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}"
            : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
    }

    public void RegisterStream() => StreamCount++;
}