namespace Audiora.Application.DTOs.Response;

public class FavoriteResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public MusicResponse? Music { get; set; }
    public DateTime CreatedAt { get; set; }
}