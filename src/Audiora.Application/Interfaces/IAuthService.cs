using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;

namespace Audiora.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<UserResponse>> GetProfileAsync(Guid userId);
}