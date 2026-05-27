using Audiora.Application.Common;
using Audiora.Application.DTOs.Response;

namespace Audiora.Application.Interfaces;

public interface IFavoriteService
{
    Task<Result<IEnumerable<FavoriteResponse>>> GetUserFavoritesAsync(Guid userId);
    Task<Result<bool>> ToggleFavoriteAsync(Guid userId, Guid musicId);
    Task<Result<bool>> IsFavoriteAsync(Guid userId, Guid musicId);
}