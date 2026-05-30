using Audiora.Domain.Enums;

namespace Audiora.Application.DTOs.Request;

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Country { get; set; }
    public UserRole Role { get; set; } = UserRole.FreeUser;
}