using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist>
{
    Task<IEnumerable<Playlist>> GetByUserAsync(Guid userId);
    Task<Playlist?> GetWithMusicsAsync(Guid playlistId);
    Task<IEnumerable<Playlist>> GetPublicAsync(int page, int pageSize);
}