using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid? MusicId { get; set; }
    public Music? Music { get; set; }

    public Guid? PodcastId { get; set; }
    public Podcast? Podcast { get; set; }

    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }
}