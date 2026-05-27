using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;

namespace Audiora.Application.Interfaces;

public interface IMusicService
{
    Task<Result<MusicResponse>> GetByIdAsync(Guid id);
    Task<Result<PagedResponse<MusicResponse>>> GetAllAsync(int page, int pageSize);
    Task<Result<PagedResponse<MusicResponse>>> SearchAsync(string term, int page, int pageSize);
    Task<Result<MusicResponse>> CreateAsync(CreateMusicRequest request, Guid artistUserId);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
    Task<Result<bool>> RegisterStreamAsync(Guid musicId);
}