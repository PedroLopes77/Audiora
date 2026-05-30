namespace Audiora.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IMusicRepository Musics { get; }
    IPlaylistRepository Playlists { get; }
    IFavoriteRepository Favorites { get; }
    IHistoryRepository Histories { get; }
    IArtistRepository Artists { get; }
    Task<int> CommitAsync();
    Task RollbackAsync();
}