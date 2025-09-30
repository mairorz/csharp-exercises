using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Data;

public static class AdminDefault
{
    public static async Task Admin(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync(u => u.Role == "admin")) { return; }

        var now = DateTime.UtcNow;

        var admin = new User
        {
            Username     = "admin",
            Name         = "Admin",
            Surname      = "System",
            Phone        = null,
            Email        = "admin@servicesmanagement.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role         = "admin",
            Status       = "active",
            CreatedAt    = now,
            UpdatedAt    = now
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
