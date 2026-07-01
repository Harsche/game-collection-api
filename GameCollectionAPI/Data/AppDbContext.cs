using GameCollectionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollectionAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add unique index for Username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Add roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)RoleType.Admin, Name = RoleType.Admin.ToString() },
            new Role { Id = (int)RoleType.User, Name = RoleType.User.ToString() }
        );
    }
}
