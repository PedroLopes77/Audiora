using Audiora.Application.Common;
using Audiora.Application.DTOs.Request;
using Audiora.Application.DTOs.Response;

namespace Audiora.Application.Interfaces;

public interface IHistoryService
{
    Task<Result<PagedResponse<HistoryResponse>>> GetUserHistoryAsync(Guid userId, int page, int pageSize);
    Task<Result<bool>> RegisterAsync(RegisterHistoryRequest request, Guid userId);
    Task<Result<IEnumerable<MusicResponse>>> GetRecommendationsAsync(Guid userId);
}