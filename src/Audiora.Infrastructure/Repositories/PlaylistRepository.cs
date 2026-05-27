using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class PlaylistRepository : BaseRepository<Playlist>, IPlaylistRepository
{
    public PlaylistRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Playlist>> GetByUserAsync(Guid userId) =>
        await _dbSet
            .Include(p => p.PlaylistMusics)
            .ThenInclude(pm => pm.Music)
            .ThenInclude(m => m.Artist)
            .Where(p => p.OwnerId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<Playlist?> GetWithMusicsAsync(Guid playlistId) =>
        await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.PlaylistMusics.OrderBy(pm => pm.Position))
            .ThenInclude(pm => pm.Music)
            .ThenInclude(m => m.Artist)
            .FirstOrDefaultAsync(p => p.Id == playlistId);

    public async Task<IEnumerable<Playlist>> GetPublicAsync(int page, int pageSize) =>
        await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.PlaylistMusics)
            .Where(p => p.IsPublic)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}