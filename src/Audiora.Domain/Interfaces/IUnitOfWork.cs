namespace Audiora.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IMusicRepository Musics { get; }
    IPlaylistRepository Playlists { get; }
    Task<int> CommitAsync();
    Task RollbackAsync();
}