namespace Audiora.Application.DTOs.Request;

public class RegisterHistoryRequest
{
    public Guid? MusicId { get; set; }
    public Guid? EpisodeId { get; set; }
    public int ProgressSeconds { get; set; }
    public bool Completed { get; set; }
}