using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public async Task<User?> GetWithSubscriptionAsync(Guid userId) =>
        await _dbSet
            .Include(u => u.Subscription)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _dbSet.AnyAsync(u => u.Email == email.ToLower());
}