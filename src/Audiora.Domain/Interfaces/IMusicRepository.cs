using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IMusicRepository : IRepository<Music>
{
    Task<IEnumerable<Music>> GetByArtistAsync(Guid artistId);
    Task<IEnumerable<Music>> SearchAsync(string term, int page, int pageSize);
    Task<IEnumerable<Music>> GetByGenreAsync(string genre, int page, int pageSize);
    Task<IEnumerable<Music>> GetMostPlayedAsync(int count);
}