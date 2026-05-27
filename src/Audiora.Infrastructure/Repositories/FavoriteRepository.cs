using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Favorite>> GetByUserAsync(Guid userId) =>
        await _dbSet
            .Include(f => f.Music).ThenInclude(m => m!.Artist)
            .Include(f => f.Album)
            .Include(f => f.Podcast)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

    public async Task<Favorite?> GetByUserAndMusicAsync(Guid userId, Guid musicId) =>
        await _dbSet.FirstOrDefaultAsync(f =>
            f.UserId == userId && f.MusicId == musicId);

    public async Task<bool> IsFavoriteAsync(Guid userId, Guid musicId) =>
        await _dbSet.AnyAsync(f =>
            f.UserId == userId && f.MusicId == musicId);
}