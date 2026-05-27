using Audiora.Domain.Enums;

namespace Audiora.Application.DTOs.Request;

public class CreateMusicRequest
{
    public string Title { get; set; } = string.Empty;
    public MusicGenre Genre { get; set; }
    public int DurationSeconds { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsExplicit { get; set; }
    public bool AllowDownload { get; set; } = true;
    public string? Lyrics { get; set; }
    public Guid? AlbumId { get; set; }
    // O arquivo de áudio será recebido separadamente via IFormFile
    public string AudioUrl { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
}