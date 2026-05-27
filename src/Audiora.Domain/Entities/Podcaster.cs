using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Podcaster : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();
}