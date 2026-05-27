using Microsoft.AspNetCore.Mvc;

namespace Audiora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<StreamController> _logger;

    public StreamController(IWebHostEnvironment env, ILogger<StreamController> logger)
    {
        _env = env;
        _logger = logger;
    }

    [HttpGet("audio/{fileName}")]
    public IActionResult StreamAudio(string fileName)
    {
        var filePath = Path.Combine(_env.ContentRootPath, "Uploads", "Audio", fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound(new { message = "Arquivo de áudio não encontrado." });

        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = ext switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            ".aac" => "audio/aac",
            _ => "application/octet-stream"
        };

        // Suporte a Range requests (necessário para seek no player)
        var fileInfo = new FileInfo(filePath);
        var rangeHeader = Request.Headers["Range"].ToString();

        if (!string.IsNullOrEmpty(rangeHeader))
        {
            return HandleRangeRequest(filePath, contentType, fileInfo.Length, rangeHeader);
        }

        _logger.LogInformation("Streaming audio: {FileName}", fileName);
        return PhysicalFile(filePath, contentType, enableRangeProcessing: true);
    }

    private IActionResult HandleRangeRequest(
        string filePath, string contentType, long fileSize, string rangeHeader)
    {
        var range = rangeHeader.Replace("bytes=", "").Split('-');
        var start = long.Parse(range[0]);
        var end = range.Length > 1 && !string.IsNullOrEmpty(range[1])
            ? long.Parse(range[1])
            : fileSize - 1;

        end = Math.Min(end, fileSize - 1);
        var length = end - start + 1;

        Response.StatusCode = 206;
        Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileSize}");
        Response.Headers.Append("Accept-Ranges", "bytes");
        Response.Headers.Append("Content-Length", length.ToString());
        Response.ContentType = contentType;

        var buffer = new byte[length];
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(start, SeekOrigin.Begin);
        fileStream.Read(buffer, 0, (int)length);

        return File(buffer, contentType);
    }
}