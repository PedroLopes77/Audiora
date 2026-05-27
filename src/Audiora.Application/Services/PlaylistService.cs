using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;
using Audiora.Application.Interfaces;
using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Application.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlaylistService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PlaylistResponse>> GetByIdAsync(Guid id)
    {
        var playlist = await _unitOfWork.Playlists.GetWithMusicsAsync(id);
        if (playlist == null)
            return Result<PlaylistResponse>.Fail("Playlist não encontrada.", "NOT_FOUND");

        return Result<PlaylistResponse>.Ok(MapToResponse(playlist));
    }

    public async Task<Result<IEnumerable<PlaylistResponse>>> GetUserPlaylistsAsync(Guid userId)
    {
        var playlists = await _unitOfWork.Playlists.GetByUserAsync(userId);
        return Result<IEnumerable<PlaylistResponse>>.Ok(
            playlists.Select(MapToResponse));
    }

    public async Task<Result<PlaylistResponse>> CreateAsync(CreatePlaylistRequest request, Guid userId)
    {
        var playlist = new Playlist
        {
            Name = request.Name,
            Description = request.Description,
            IsPublic = request.IsPublic,
            IsCollaborative = request.IsCollaborative,
            OwnerId = userId
        };

        await _unitOfWork.Playlists.AddAsync(playlist);
        await _unitOfWork.CommitAsync();

        return Result<PlaylistResponse>.Ok(MapToResponse(playlist));
    }

    public async Task<Result<bool>> AddMusicAsync(
        Guid playlistId, AddMusicToPlaylistRequest request, Guid userId)
    {
        var playlist = await _unitOfWork.Playlists.GetWithMusicsAsync(playlistId);
        if (playlist == null)
            return Result<bool>.Fail("Playlist não encontrada.", "NOT_FOUND");

        if (playlist.OwnerId != userId && !playlist.IsCollaborative)
            return Result<bool>.Fail("Sem permissão para editar esta playlist.", "FORBIDDEN");

        var alreadyExists = playlist.PlaylistMusics
            .Any(pm => pm.MusicId == request.MusicId);

        if (alreadyExists)
            return Result<bool>.Fail("Música já está na playlist.", "ALREADY_EXISTS");

        var music = await _unitOfWork.Musics.GetByIdAsync(request.MusicId);
        if (music == null)
            return Result<bool>.Fail("Música não encontrada.", "NOT_FOUND");

        playlist.PlaylistMusics.Add(new PlaylistMusic
        {
            PlaylistId = playlistId,
            MusicId = request.MusicId,
            Position = request.Position,
            AddedByUserId = userId
        });

        await _unitOfWork.Playlists.UpdateAsync(playlist);
        await _unitOfWork.CommitAsync();

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> RemoveMusicAsync(Guid playlistId, Guid musicId, Guid userId)
    {
        var playlist = await _unitOfWork.Playlists.GetWithMusicsAsync(playlistId);
        if (playlist == null)
            return Result<bool>.Fail("Playlist não encontrada.", "NOT_FOUND");

        if (playlist.OwnerId != userId)
            return Result<bool>.Fail("Sem permissão.", "FORBIDDEN");

        var item = playlist.PlaylistMusics.FirstOrDefault(pm => pm.MusicId == musicId);
        if (item == null)
            return Result<bool>.Fail("Música não está na playlist.", "NOT_FOUND");

        playlist.PlaylistMusics.Remove(item);
        await _unitOfWork.CommitAsync();

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid playlistId, Guid userId)
    {
        var playlist = await _unitOfWork.Playlists.GetByIdAsync(playlistId);
        if (playlist == null)
            return Result<bool>.Fail("Playlist não encontrada.", "NOT_FOUND");

        if (playlist.OwnerId != userId)
            return Result<bool>.Fail("Sem permissão.", "FORBIDDEN");

        await _unitOfWork.Playlists.DeleteAsync(playlist);
        await _unitOfWork.CommitAsync();

        return Result<bool>.Ok(true);
    }

    private static PlaylistResponse MapToResponse(Playlist playlist) => new()
    {
        Id = playlist.Id,
        Name = playlist.Name,
        Description = playlist.Description,
        CoverImageUrl = playlist.CoverImageUrl,
        IsPublic = playlist.IsPublic,
        IsCollaborative = playlist.IsCollaborative,
        OwnerName = playlist.Owner?.Name ?? string.Empty,
        TotalTracks = playlist.PlaylistMusics.Count,
        CreatedAt = playlist.CreatedAt,
        Musics = playlist.PlaylistMusics
            .OrderBy(pm => pm.Position)
            .Where(pm => pm.Music != null)
            .Select(pm => new MusicResponse
            {
                Id = pm.Music!.Id,
                Title = pm.Music.Title,
                AudioUrl = pm.Music.AudioUrl,
                CoverImageUrl = pm.Music.CoverImageUrl,
                DurationSeconds = pm.Music.DurationSeconds,
                Duration = pm.Music.GetFormattedDuration(),
                Genre = pm.Music.Genre.ToString(),
                StreamCount = pm.Music.StreamCount,
                ArtistId = pm.Music.ArtistId,
                ArtistName = pm.Music.Artist?.Name ?? string.Empty
            }).ToList()
    };
}