// src/Audiora.Application/Interfaces/IStorageService.cs
namespace Audiora.Application.Interfaces;

public interface IStorageService
{
    Task<string> SaveAudioAsync(Stream fileStream, string fileName);
    Task<string> SaveImageAsync(Stream fileStream, string fileName);
    Task<bool> DeleteFileAsync(string fileUrl);
    string GetFileUrl(string relativePath);
}