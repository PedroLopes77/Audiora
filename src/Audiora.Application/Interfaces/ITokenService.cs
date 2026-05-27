using Audiora.Domain.Entities;

namespace Audiora.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}