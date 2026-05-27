using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Album : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int TotalTracks { get; set; }

    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;

    public ICollection<Music> Musics { get; set; } = new List<Music>();
}