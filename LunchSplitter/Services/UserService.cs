using System.Security.Cryptography;
using LunchSplitter.Data;
using LunchSplitter.Domain.Entity;
using LunchSplitter.Models.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace LunchSplitter.Services;

public class UserService
{
    private IDbContextFactory<DatabaseContext> _dbContextFactory;

    public UserService(IDbContextFactory<DatabaseContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public List<User> GetUsers()
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Users.ToList();
    }

    public User GetUserByUsername(string name)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return context.Users.FirstOrDefault(u => u.Name == name);
        }
    }

    public async Task<User> AddUser(SignupViewModel newUser)
    {
        var hash = HashPassword(newUser.Password);
        var user = new User
        {
            Name = newUser.Username,
            Email = newUser.Email,
            Password = hash.hashed,
            Salt = Convert.ToBase64String(hash.salt)
        };
        
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        
        return user;
    }

    
    public readonly record struct Hash(byte[] salt, string hashed);
    public static Hash HashPassword(string password, string? savedSalt = null)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        
        if (savedSalt != null)
        {
            salt = Convert.FromBase64String(savedSalt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return new Hash(salt, hashed);
    }
    
    public static bool CompareHash(string attemptedPassword, string hash, string salt)
    {
        string base64Hash = hash;
        string base64AttemptedHash = HashPassword(attemptedPassword, salt).hashed;

        return base64Hash == base64AttemptedHash;
    }
}