using Audiora.Domain.Entities;
using Audiora.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Audiora.Infrastructure.Data;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var artistUserId = Guid.NewGuid();

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Admin Audiora",
            Email = "admin@audiora.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@2024!"),
            Role = UserRole.Admin,
            BirthDate = new DateTime(1990, 1, 1),
            Country = "BR"
        };

        var artistUser = new User
        {
            Id = artistUserId,
            Name = "Artista Demo",
            Email = "artista@audiora.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Artista@2024!"),
            Role = UserRole.Artist,
            BirthDate = new DateTime(1995, 6, 15),
            Country = "BR"
        };

        var artist = new Artist
        {
            Name = "Artista Demo",
            Bio = "Artista de demonstração da plataforma Audiora.",
            Country = "BR",
            Genre = "Pop",
            IsVerified = true,
            UserId = artistUserId
        };

        await context.Users.AddRangeAsync(admin, artistUser);
        await context.Artists.AddAsync(artist);
        await context.SaveChangesAsync();
    }
}