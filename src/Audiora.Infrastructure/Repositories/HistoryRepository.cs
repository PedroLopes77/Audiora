using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class HistoryRepository : BaseRepository<History>, IHistoryRepository
{
    public HistoryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<History>> GetByUserAsync(Guid userId, int page, int pageSize) =>
        await _dbSet
            .Include(h => h.Music).ThenInclude(m => m!.Artist)
            .Include(h => h.Episode).ThenInclude(e => e!.Podcast)
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.PlayedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<History?> GetByUserAndMusicAsync(Guid userId, Guid musicId) =>
        await _dbSet.FirstOrDefaultAsync(h =>
            h.UserId == userId && h.MusicId == musicId);

    public async Task<IEnumerable<Music>> GetMostPlayedByUserAsync(Guid userId, int count) =>
        await _dbSet
            .Include(h => h.Music).ThenInclude(m => m!.Artist)
            .Where(h => h.UserId == userId && h.MusicId != null)
            .GroupBy(h => h.MusicId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.First().Music!)
            .ToListAsync();
}