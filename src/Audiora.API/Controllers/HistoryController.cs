using Audiora.Application.DTOs.Request;
using Audiora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Audiora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _historyService.GetUserHistoryAsync(GetUserId(), page, pageSize);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterHistoryRequest request)
    {
        var result = await _historyService.RegisterAsync(request, GetUserId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations()
    {
        var result = await _historyService.GetRecommendationsAsync(GetUserId());
        return Ok(result);
    }
}