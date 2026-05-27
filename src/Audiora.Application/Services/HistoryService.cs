using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;
using Audiora.Application.Interfaces;
using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;

namespace Audiora.Application.Services;

public class HistoryService : IHistoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public HistoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResponse<HistoryResponse>>> GetUserHistoryAsync(
        Guid userId, int page, int pageSize)
    {
        var histories = await _unitOfWork.Histories.GetByUserAsync(userId, page, pageSize);
        var total = await _unitOfWork.Histories.CountAsync(h => h.UserId == userId);

        var response = histories.Select(h => new HistoryResponse
        {
            Id = h.Id,
            PlayedAt = h.PlayedAt,
            ProgressSeconds = h.ProgressSeconds,
            Completed = h.Completed,
            Music = h.Music == null ? null : new MusicResponse
            {
                Id = h.Music.Id,
                Title = h.Music.Title,
                AudioUrl = h.Music.AudioUrl,
                CoverImageUrl = h.Music.CoverImageUrl,
                DurationSeconds = h.Music.DurationSeconds,
                Duration = h.Music.GetFormattedDuration(),
                Genre = h.Music.Genre.ToString(),
                StreamCount = h.Music.StreamCount,
                ArtistId = h.Music.ArtistId,
                ArtistName = h.Music.Artist?.Name ?? string.Empty
            }
        });

        return Result<PagedResponse<HistoryResponse>>.Ok(new PagedResponse<HistoryResponse>
        {
            Data = response,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        });
    }

    public async Task<Result<bool>> RegisterAsync(RegisterHistoryRequest request, Guid userId)
    {
        // Atualiza se já existe, senão cria
        if (request.MusicId.HasValue)
        {
            var existing = await _unitOfWork.Histories
                .GetByUserAndMusicAsync(userId, request.MusicId.Value);

            if (existing != null)
            {
                existing.ProgressSeconds = request.ProgressSeconds;
                existing.Completed = request.Completed;
                existing.PlayedAt = DateTime.UtcNow;
                await _unitOfWork.Histories.UpdateAsync(existing);
                await _unitOfWork.CommitAsync();
                return Result<bool>.Ok(true);
            }
        }

        await _unitOfWork.Histories.AddAsync(new History
        {
            UserId = userId,
            MusicId = request.MusicId,
            EpisodeId = request.EpisodeId,
            ProgressSeconds = request.ProgressSeconds,
            Completed = request.Completed,
            PlayedAt = DateTime.UtcNow
        });

        await _unitOfWork.CommitAsync();
        return Result<bool>.Ok(true);
    }

    public async Task<Result<IEnumerable<MusicResponse>>> GetRecommendationsAsync(Guid userId)
    {
        // Recomendação simples: músicas mais ouvidas pelo usuário + mais tocadas globalmente
        var userFavorites = await _unitOfWork.Histories
            .GetMostPlayedByUserAsync(userId, 5);

        if (!userFavorites.Any())
        {
            var popular = await _unitOfWork.Musics.GetMostPlayedAsync(10);
            return Result<IEnumerable<MusicResponse>>.Ok(
                popular.Select(MapMusicResponse));
        }

        return Result<IEnumerable<MusicResponse>>.Ok(
            userFavorites.Select(MapMusicResponse));
    }

    private static MusicResponse MapMusicResponse(Music m) => new()
    {
        Id = m.Id,
        Title = m.Title,
        AudioUrl = m.AudioUrl,
        CoverImageUrl = m.CoverImageUrl,
        DurationSeconds = m.DurationSeconds,
        Duration = m.GetFormattedDuration(),
        Genre = m.Genre.ToString(),
        StreamCount = m.StreamCount,
        ArtistId = m.ArtistId,
        ArtistName = m.Artist?.Name ?? string.Empty
    };
}