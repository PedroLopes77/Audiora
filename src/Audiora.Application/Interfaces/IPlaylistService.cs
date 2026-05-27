using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;

namespace Audiora.Application.Interfaces;

public interface IPlaylistService
{
    Task<Result<PlaylistResponse>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<PlaylistResponse>>> GetUserPlaylistsAsync(Guid userId);
    Task<Result<PlaylistResponse>> CreateAsync(CreatePlaylistRequest request, Guid userId);
    Task<Result<bool>> AddMusicAsync(Guid playlistId, AddMusicToPlaylistRequest request, Guid userId);
    Task<Result<bool>> RemoveMusicAsync(Guid playlistId, Guid musicId, Guid userId);
    Task<Result<bool>> DeleteAsync(Guid playlistId, Guid userId);
}