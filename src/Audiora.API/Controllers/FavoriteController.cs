using Audiora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Audiora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoriteController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyFavorites()
    {
        var result = await _favoriteService.GetUserFavoritesAsync(GetUserId());
        return Ok(result);
    }

    [HttpPost("{musicId:guid}/toggle")]
    public async Task<IActionResult> Toggle(Guid musicId)
    {
        var result = await _favoriteService.ToggleFavoriteAsync(GetUserId(), musicId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("{musicId:guid}/check")]
    public async Task<IActionResult> Check(Guid musicId)
    {
        var result = await _favoriteService.IsFavoriteAsync(GetUserId(), musicId);
        return Ok(result);
    }
}