using Audiora.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Audiora.Infrastructure.Services;

public class StorageService : IStorageService
{
    private readonly string _audioPath;
    private readonly string _imagePath;
    private readonly string _baseUrl;

    public StorageService(IConfiguration configuration)
    {
        _audioPath = configuration["StorageSettings:AudioPath"] ?? "Uploads/Audio";
        _imagePath = configuration["StorageSettings:ImagePath"] ?? "Uploads/Images";
        _baseUrl = configuration["StorageSettings:BaseUrl"] ?? "http://localhost:5003";
    }

    public async Task<string> SaveAudioAsync(Stream fileStream, string fileName)
    {
        ValidateAudioExtension(fileName);
        return await SaveFileAsync(fileStream, fileName, _audioPath);
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName)
    {
        ValidateImageExtension(fileName);
        return await SaveFileAsync(fileStream, fileName, _imagePath);
    }

    public Task<bool> DeleteFileAsync(string fileUrl)
    {
        var relativePath = fileUrl.Replace(_baseUrl + "/", "");
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.FromResult(true);
    }

    public string GetFileUrl(string relativePath) =>
        $"{_baseUrl}/{relativePath.Replace("\\", "/")}";

    private async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder)
    {
        var fullFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
        Directory.CreateDirectory(fullFolder);

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var newFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(fullFolder, newFileName);

        using var output = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(output);

        return GetFileUrl(Path.Combine(folder, newFileName));
    }

    private static void ValidateAudioExtension(string fileName)
    {
        var allowed = new[] { ".mp3", ".wav", ".ogg", ".flac", ".aac" };
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            throw new InvalidOperationException($"Formato não permitido: {ext}");
    }

    private static void ValidateImageExtension(string fileName)
    {
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            throw new InvalidOperationException($"Formato não permitido: {ext}");
    }
}