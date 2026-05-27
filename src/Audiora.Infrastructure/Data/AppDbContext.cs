using Audiora.Domain.Entities;
using Audiora.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Podcaster> Podcasters => Set<Podcaster>();
    public DbSet<Music> Musics => Set<Music>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<PlaylistMusic> PlaylistMusics => Set<PlaylistMusic>();
    public DbSet<Podcast> Podcasts => Set<Podcast>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<History> Histories => Set<History>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações da pasta Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Filtro global: nunca retorna registros deletados
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Music>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Artist>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Album>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Playlist>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Podcast>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Episode>().HasQueryFilter(e => !e.IsDeleted);
    }

    // Intercepta SaveChanges para atualizar UpdatedAt automaticamente
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Domain.Common.BaseEntity entity)
            {
                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}