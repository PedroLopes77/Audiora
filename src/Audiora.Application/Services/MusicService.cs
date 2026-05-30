using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;
using Audiora.Application.Interfaces;
using Audiora.Domain.Entities;
using Audiora.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Application.Services;

public class MusicService : IMusicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MusicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<MusicResponse>> GetByIdAsync(Guid id)
    {
        var music = await _unitOfWork.Musics
            .Query()
            .Include(m => m.Artist)
            .Include(m => m.Album)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (music == null)
            return Result<MusicResponse>.Fail("Música não encontrada.", "NOT_FOUND");

        return Result<MusicResponse>.Ok(_mapper.Map<MusicResponse>(music));
    }

    public async Task<Result<PagedResponse<MusicResponse>>> GetAllAsync(int page, int pageSize)
    {
        var total = await _unitOfWork.Musics.CountAsync();

        var musics = await _unitOfWork.Musics
            .Query()
            .Include(m => m.Artist)
            .Include(m => m.Album)
            .OrderByDescending(m => m.StreamCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Result<PagedResponse<MusicResponse>>.Ok(new PagedResponse<MusicResponse>
        {
            Data = _mapper.Map<IEnumerable<MusicResponse>>(musics),
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        });
    }

    public async Task<Result<PagedResponse<MusicResponse>>> SearchAsync(string term, int page, int pageSize)
    {
        var query = _unitOfWork.Musics
            .Query()
            .Include(m => m.Artist)
            .Where(m => m.Title.Contains(term) || m.Artist.Name.Contains(term));

        var total = await query.CountAsync();

        var musics = await query
            .OrderByDescending(m => m.StreamCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Result<PagedResponse<MusicResponse>>.Ok(new PagedResponse<MusicResponse>
        {
            Data = _mapper.Map<IEnumerable<MusicResponse>>(musics),
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        });
    }

    public async Task<Result<MusicResponse>> CreateAsync(CreateMusicRequest request, Guid artistUserId)
    {
        var artist = await _unitOfWork.Artists.GetByUserIdAsync(artistUserId);

        if (artist == null)
            return Result<MusicResponse>.Fail("Artista não encontrado.", "NOT_FOUND");

        var music = new Music
        {
            Title = request.Title,
            AudioUrl = request.AudioUrl,
            CoverImageUrl = request.CoverImageUrl,
            DurationSeconds = request.DurationSeconds,
            Genre = request.Genre,
            IsExplicit = request.IsExplicit,
            AllowDownload = request.AllowDownload,
            Lyrics = request.Lyrics,
            ReleaseYear = request.ReleaseYear,
            AlbumId = request.AlbumId,
            ArtistId = artist.Id
        };

        await _unitOfWork.Musics.AddAsync(music);
        await _unitOfWork.CommitAsync();

        return Result<MusicResponse>.Ok(_mapper.Map<MusicResponse>(music));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var music = await _unitOfWork.Musics.GetByIdAsync(id);
        if (music == null)
            return Result<bool>.Fail("Música não encontrada.", "NOT_FOUND");

        await _unitOfWork.Musics.DeleteAsync(music);
        await _unitOfWork.CommitAsync();

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> RegisterStreamAsync(Guid musicId)
    {
        var music = await _unitOfWork.Musics.GetByIdAsync(musicId);
        if (music == null)
            return Result<bool>.Fail("Música não encontrada.", "NOT_FOUND");

        music.RegisterStream();
        await _unitOfWork.Musics.UpdateAsync(music);
        await _unitOfWork.CommitAsync();

        return Result<bool>.Ok(true);
    }
}