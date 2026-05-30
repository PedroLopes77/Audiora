using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
{
    public ArtistRepository(AppDbContext context) : base(context) { }

    public async Task<Artist?> GetByUserIdAsync(Guid userId)
        => await _dbSet.FirstOrDefaultAsync(a => a.UserId == userId);
}