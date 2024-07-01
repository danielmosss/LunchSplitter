using System.Security.Cryptography;
using a;
using LunchSplitter.Domain.Entity;
using LunchSplitter.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;


namespace LunchSplitter.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupUserPermissions> GroupUserPermissions { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionShare> TransactionShares { get; set; }
    
    public DbSet<GroupSharepreset> GroupSharepresets { get; set; }
    public DbSet<Sharepreset> Sharepresets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var hash = UserService.HashPassword("Admin@1234");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "SystemAdmin",
                Email = "admin@example.com",
                Password = hash.hashed,
                Salt = hash.salt.ToString()
            });
    }
}