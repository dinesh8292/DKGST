using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DKGST.Core.Services;

public class UserService : IUserService
{
    private readonly MasterDbContext _context;

    public UserService(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserCompanies)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserCompanies)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        user.PasswordHash = HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await GetUserByIdAsync(userId);
        if (user == null) return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> VerifyPasswordAsync(User user, string password)
    {
        var result = VerifyHash(password, user.PasswordHash);
        return Task.FromResult(result);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(256 / 8);
        byte[] hashWithSalt = new byte[salt.Length + hash.Length];
        Array.Copy(salt, 0, hashWithSalt, 0, salt.Length);
        Array.Copy(hash, 0, hashWithSalt, salt.Length, hash.Length);
        return Convert.ToBase64String(hashWithSalt);
    }

    private static bool VerifyHash(string password, string hash)
    {
        byte[] hashWithSalt = Convert.FromBase64String(hash);
        int saltLength = 128 / 8;
        byte[] salt = new byte[saltLength];
        Array.Copy(hashWithSalt, 0, salt, 0, saltLength);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash2 = pbkdf2.GetBytes(256 / 8);

        for (int i = 0; i < hash2.Length; i++)
        {
            if (hashWithSalt[i + saltLength] != hash2[i])
                return false;
        }
        return true;
    }
}
