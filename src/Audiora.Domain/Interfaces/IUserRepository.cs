using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetWithSubscriptionAsync(Guid userId);
    Task<bool> EmailExistsAsync(string email);
}