using Audiora.Domain.Common;

namespace Audiora.Domain.Entities;

public class Artist : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Country { get; set; }
    public string? Genre { get; set; }
    public bool IsVerified { get; set; } = false;
    public long TotalStreams { get; set; } = 0;
    public decimal TotalRevenue { get; set; } = 0;

    // Vínculo com usuário (artista também é usuário)
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Navigation properties
    public ICollection<Music> Musics { get; set; } = new List<Music>();
    public ICollection<Album> Albums { get; set; } = new List<Album>();

    // Regra: R$ 0,004 por stream
    public decimal CalculateRevenue(long streams) => streams * 0.004m;
}