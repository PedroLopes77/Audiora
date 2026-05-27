namespace Audiora.Domain.Entities;

// Tabela de junção com dados extras
public class PlaylistMusic
{
    public Guid PlaylistId { get; set; }
    public Playlist Playlist { get; set; } = null!;

    public Guid MusicId { get; set; }
    public Music Music { get; set; } = null!;

    public int Position { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public Guid AddedByUserId { get; set; }
}