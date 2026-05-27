using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;

namespace Audiora.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IMusicRepository Musics { get; }
    public IPlaylistRepository Playlists { get; }

    public UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IMusicRepository musics,
        IPlaylistRepository playlists)
    {
        _context = context;
        Users = users;
        Musics = musics;
        Playlists = playlists;
    }

    public async Task<int> CommitAsync() =>
        await _context.SaveChangesAsync();

    public async Task RollbackAsync() =>
        await Task.CompletedTask;

    public void Dispose() => _context.Dispose();
}