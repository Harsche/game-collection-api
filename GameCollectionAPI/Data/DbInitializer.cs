
using GameCollectionAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameCollectionAPI.Data;

public static class DbInitializer
{

    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        // Database migration
        await context.Database.MigrateAsync();

        // Seed admin user
        if (!await context.Users.AnyAsync(u => u.RoleId == (int)RoleType.Admin))
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var adminUsername = configuration["DefaultAdminUsername"] ?? "Admin";
            var adminPassword = configuration["DefaultAdminPassword"];

            if (adminPassword == null)
            {
                throw new Exception("Default admin password not provided.");
            }

            var adminUser = new User
            {
                Id = -1,
                Username = adminUsername,
                CreatedDate = DateTime.UtcNow,
                RoleId = (int)RoleType.Admin
            };

            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, adminPassword);

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}