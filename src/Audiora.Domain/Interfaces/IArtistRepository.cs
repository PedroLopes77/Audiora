using Audiora.Domain.Entities;

namespace Audiora.Domain.Interfaces;

public interface IArtistRepository : IRepository<Artist>
{
    Task<Artist?> GetByUserIdAsync(Guid userId);
}