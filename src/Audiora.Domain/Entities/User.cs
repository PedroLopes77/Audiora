using Audiora.Domain.Common;
using Audiora.Domain.Enums;

namespace Audiora.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Country { get; set; }
    public UserRole Role { get; set; } = UserRole.FreeUser;
    public int SkipCount { get; set; } = 0;
    public DateTime? SkipCountResetAt { get; set; }

    // Navigation properties
    public Subscription? Subscription { get; set; }
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<History> Histories { get; set; } = new List<History>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // Regra de negócio: plano free tem 6 skips por hora
    public bool CanSkip()
    {
        if (Role == UserRole.PremiumUser || Role == UserRole.Admin) return true;

        if (SkipCountResetAt == null || DateTime.UtcNow >= SkipCountResetAt)
        {
            SkipCount = 0;
            SkipCountResetAt = DateTime.UtcNow.AddHours(1);
            return true;
        }

        return SkipCount < 6;
    }

    public void RegisterSkip()
    {
        SkipCount++;
    }

    public bool IsPremium() =>
        Subscription != null &&
        Subscription.IsActive &&
        Subscription.ExpiresAt > DateTime.UtcNow;
}