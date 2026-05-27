namespace Audiora.Application.DTOs.Response;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? Country { get; set; }
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; }
}