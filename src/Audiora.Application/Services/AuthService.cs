using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;
using Audiora.Application.Interfaces;
using Audiora.Domain.Entities;
using Audiora.Domain.Enums;
using Audiora.Domain.Interfaces;
using AutoMapper;

namespace Audiora.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.ToLower());
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Fail("Email ou senha invalidos.", "INVALID_CREDENTIALS");

        if (!user.IsActive)
            return Result<AuthResponse>.Fail("Conta desativada.", "ACCOUNT_DISABLED");

        var token = _tokenService.GenerateToken(user);
        return Result<AuthResponse>.Ok(new AuthResponse
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsPremium = user.IsPremium(),
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        });
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email.ToLower()))
            return Result<AuthResponse>.Fail("Email ja cadastrado.", "EMAIL_EXISTS");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            BirthDate = request.BirthDate,
            Country = request.Country,
            Role = request.Role
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CommitAsync();

        if (user.Role == UserRole.Artist)
        {
            var artist = new Artist
            {
                Name = user.Name,
                Country = user.Country,
                UserId = user.Id
            };
            await _unitOfWork.Artists.AddAsync(artist);
            await _unitOfWork.CommitAsync();
        }

        var token = _tokenService.GenerateToken(user);
        return Result<AuthResponse>.Ok(new AuthResponse
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsPremium = false,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        });
    }

    public async Task<Result<UserResponse>> GetProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetWithSubscriptionAsync(userId);
        if (user == null)
            return Result<UserResponse>.Fail("Usuario nao encontrado.", "NOT_FOUND");

        return Result<UserResponse>.Ok(_mapper.Map<UserResponse>(user));
    }
}