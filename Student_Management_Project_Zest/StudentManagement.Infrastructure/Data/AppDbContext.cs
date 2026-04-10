using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;

namespace StudentManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();
            
            // Seed a default admin user
            // Password is 'admin123' hashed using BCrypt or similar, but for simplicity we'll just seed a plain or simple hash 
            // Better to use Bcrypt.Net-Next logic or let auth service handle it. For assignments, let's keep it simple.
            // Let's seed a user where Username="admin", PasswordHash="AQAAAAIAAYagAAAAEGn8mI..." (we'll provide a fixed hash corresponding to 'password')
            // Actually, we can just insert a plain string if we use plain comparison for demo, or a known hash. 
            // I'll leave the hash logic to the auth controller and insert seed data.
        });
    }
}
