namespace Audiora.Application.DTOs.Response;

public class PlaylistResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public bool IsCollaborative { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public int TotalTracks { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MusicResponse> Musics { get; set; } = new();
}