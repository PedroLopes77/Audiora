using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class History : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid? MusicId { get; set; }
    public Music? Music { get; set; }

    public Guid? EpisodeId { get; set; }
    public Episode? Episode { get; set; }

    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    public int ProgressSeconds { get; set; } = 0;
    public bool Completed { get; set; } = false;
}