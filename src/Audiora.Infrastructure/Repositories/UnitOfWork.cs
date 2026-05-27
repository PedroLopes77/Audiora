using Audiora.Domain.Interfaces;
using Audiora.Infrastructure.Data;

namespace Audiora.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IMusicRepository Musics { get; }

    public UnitOfWork(AppDbContext context,
        IUserRepository users,
        IMusicRepository musics)
    {
        _context = context;
        Users = users;
        Musics = musics;
    }

    public async Task<int> CommitAsync() =>
        await _context.SaveChangesAsync();

    public async Task RollbackAsync() =>
        await Task.CompletedTask; // EF Core não tem rollback explícito sem transação

    public void Dispose() => _context.Dispose();
}