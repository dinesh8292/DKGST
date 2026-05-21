using DKGST.Core.Models;

namespace DKGST.Core.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user, string password);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> VerifyPasswordAsync(User user, string password);
}
