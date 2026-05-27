using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<IEnumerable<Favorite>> GetByUserAsync(Guid userId);
    Task<Favorite?> GetByUserAndMusicAsync(Guid userId, Guid musicId);
    Task<bool> IsFavoriteAsync(Guid userId, Guid musicId);
}