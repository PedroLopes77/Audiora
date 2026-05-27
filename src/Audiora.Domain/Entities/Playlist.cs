using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Playlist : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsCollaborative { get; set; } = false;

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<PlaylistMusic> PlaylistMusics { get; set; } = new List<PlaylistMusic>();

    public int TotalTracks => PlaylistMusics.Count;
    public int TotalDurationSeconds => PlaylistMusics.Sum(pm => pm.Music?.DurationSeconds ?? 0);
}