using DKGST.Core.Models;

namespace DKGST.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
    Task LogoutAsync(Guid userId);
    string GenerateToken(User user);
}
