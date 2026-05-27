using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Episode : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AudioUrl { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public int EpisodeNumber { get; set; }
    public DateTime PublishedAt { get; set; }
    public long ListenCount { get; set; } = 0;

    public Guid PodcastId { get; set; }
    public Podcast Podcast { get; set; } = null!;
}