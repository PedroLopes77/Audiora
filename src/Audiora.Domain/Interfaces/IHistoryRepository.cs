using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IHistoryRepository : IRepository<History>
{
    Task<IEnumerable<History>> GetByUserAsync(Guid userId, int page, int pageSize);
    Task<History?> GetByUserAndMusicAsync(Guid userId, Guid musicId);
    Task<IEnumerable<Music>> GetMostPlayedByUserAsync(Guid userId, int count);
}