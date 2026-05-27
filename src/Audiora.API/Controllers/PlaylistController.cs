using Audiora.Application.DTOs.Request;
using Audiora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Audiora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _playlistService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyPlaylists()
    {
        var result = await _playlistService.GetUserPlaylistsAsync(GetUserId());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlaylistRequest request)
    {
        var result = await _playlistService.CreateAsync(request, GetUserId());
        return result.Success ? Created($"/api/playlist/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/musics")]
    public async Task<IActionResult> AddMusic(Guid id, [FromBody] AddMusicToPlaylistRequest request)
    {
        var result = await _playlistService.AddMusicAsync(id, request, GetUserId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}/musics/{musicId:guid}")]
    public async Task<IActionResult> RemoveMusic(Guid id, Guid musicId)
    {
        var result = await _playlistService.RemoveMusicAsync(id, musicId, GetUserId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _playlistService.DeleteAsync(id, GetUserId());
        return result.Success ? Ok(result) : NotFound(result);
    }
}