namespace Audiora.Application.DTOs.Response;

public class MusicResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public int DurationSeconds { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public long StreamCount { get; set; }
    public bool IsExplicit { get; set; }
    public bool AllowDownload { get; set; }
    public int ReleaseYear { get; set; }
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string? AlbumTitle { get; set; }
}