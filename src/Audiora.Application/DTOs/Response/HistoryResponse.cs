namespace Audiora.Application.DTOs.Response;

public class HistoryResponse
{
    public Guid Id { get; set; }
    public MusicResponse? Music { get; set; }
    public DateTime PlayedAt { get; set; }
    public int ProgressSeconds { get; set; }
    public bool Completed { get; set; }
}