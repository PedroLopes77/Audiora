using Audiora.Application.Common;
using Audiora.Application.DTOs.Response;
using Audiora.Application.Interfaces;
using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;

namespace Audiora.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IUnitOfWork _unitOfWork;

    public FavoriteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<FavoriteResponse>>> GetUserFavoritesAsync(Guid userId)
    {
        var favorites = await _unitOfWork.Favorites.GetByUserAsync(userId);

        var response = favorites.Select(f => new FavoriteResponse
        {
            Id = f.Id,
            UserId = f.UserId,
            CreatedAt = f.CreatedAt,
            Music = f.Music == null ? null : new MusicResponse
            {
                Id = f.Music.Id,
                Title = f.Music.Title,
                AudioUrl = f.Music.AudioUrl,
                CoverImageUrl = f.Music.CoverImageUrl,
                DurationSeconds = f.Music.DurationSeconds,
                Duration = f.Music.GetFormattedDuration(),
                Genre = f.Music.Genre.ToString(),
                StreamCount = f.Music.StreamCount,
                ArtistId = f.Music.ArtistId,
                ArtistName = f.Music.Artist?.Name ?? string.Empty
            }
        });

        return Result<IEnumerable<FavoriteResponse>>.Ok(response);
    }

    public async Task<Result<bool>> ToggleFavoriteAsync(Guid userId, Guid musicId)
    {
        var existing = await _unitOfWork.Favorites.GetByUserAndMusicAsync(userId, musicId);

        if (existing != null)
        {
            await _unitOfWork.Favorites.DeleteAsync(existing);
            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(false); // removido
        }

        var music = await _unitOfWork.Musics.GetByIdAsync(musicId);
        if (music == null)
            return Result<bool>.Fail("Música não encontrada.", "NOT_FOUND");

        await _unitOfWork.Favorites.AddAsync(new Favorite
        {
            UserId = userId,
            MusicId = musicId
        });

        await _unitOfWork.CommitAsync();
        return Result<bool>.Ok(true); // adicionado
    }

    public async Task<Result<bool>> IsFavoriteAsync(Guid userId, Guid musicId)
    {
        var result = await _unitOfWork.Favorites.IsFavoriteAsync(userId, musicId);
        return Result<bool>.Ok(result);
    }
}