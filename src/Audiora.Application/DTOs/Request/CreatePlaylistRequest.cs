namespace Audiora.Application.DTOs.Request;

public class CreatePlaylistRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsCollaborative { get; set; } = false;
}

public class AddMusicToPlaylistRequest
{
    public Guid MusicId { get; set; }
    public int Position { get; set; }
}