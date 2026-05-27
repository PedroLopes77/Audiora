using Audiora.Application.DTOs.Request;
using Audiora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Audiora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicController : ControllerBase
{
    private readonly IMusicService _musicService;

    public MusicController(IMusicService musicService)
    {
        _musicService = musicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _musicService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _musicService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string term,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest(new { message = "Termo de busca é obrigatório." });

        var result = await _musicService.SearchAsync(term, page, pageSize);
        return Ok(result);
    }

    [HttpPost("{id:guid}/stream")]
    public async Task<IActionResult> RegisterStream(Guid id)
    {
        var result = await _musicService.RegisterStreamAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Artist,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _musicService.DeleteAsync(id, userId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Artist,Admin")]
    public async Task<IActionResult> Upload(
        [FromForm] CreateMusicRequest request,
        IFormFile audioFile,
        IFormFile? coverImage,
        [FromServices] IStorageService storageService)
    {
        var audioUrl = await storageService.SaveAudioAsync(
            audioFile.OpenReadStream(), audioFile.FileName);
        request.AudioUrl = audioUrl;

        if (coverImage != null)
            request.CoverImageUrl = await storageService.SaveImageAsync(
                coverImage.OpenReadStream(), coverImage.FileName);

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _musicService.CreateAsync(request, userId);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}