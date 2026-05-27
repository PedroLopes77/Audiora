using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Repositories;

public class MusicRepository : BaseRepository<Music>, IMusicRepository
{
    public MusicRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Music>> GetByArtistAsync(Guid artistId) =>
        await _dbSet
            .Include(m => m.Artist)
            .Where(m => m.ArtistId == artistId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Music>> SearchAsync(string term, int page, int pageSize) =>
        await _dbSet
            .Include(m => m.Artist)
            .Where(m => m.Title.Contains(term) || m.Artist.Name.Contains(term))
            .OrderByDescending(m => m.StreamCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Music>> GetByGenreAsync(string genre, int page, int pageSize) =>
        await _dbSet
            .Include(m => m.Artist)
            .Where(m => m.Genre.ToString() == genre)
            .OrderByDescending(m => m.StreamCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Music>> GetMostPlayedAsync(int count) =>
        await _dbSet
            .Include(m => m.Artist)
            .OrderByDescending(m => m.StreamCount)
            .Take(count)
            .ToListAsync();
}