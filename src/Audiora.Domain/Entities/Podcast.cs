using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Podcast : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; }

    public Guid PodcasterId { get; set; }
    public Podcaster Podcaster { get; set; } = null!;

    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
}