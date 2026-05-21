using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace DKGST.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ICompanyService _companyService;
    private readonly IConfiguration _configuration;

    public AuthService(IUserService userService, ICompanyService companyService, IConfiguration configuration)
    {
        _userService = userService;
        _companyService = companyService;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
                return new LoginResponse { Success = false, Message = "Invalid username or password" };

            var isPasswordValid = await _userService.VerifyPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return new LoginResponse { Success = false, Message = "Invalid username or password" };

            if (!user.IsActive)
                return new LoginResponse { Success = false, Message = "User account is inactive" };

            var token = GenerateToken(user);
            var companies = await _companyService.GetUserCompaniesAsync(user.Id);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredLanguage = user.PreferredLanguage,
                Companies = companies
            };

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                User = userDto
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse { Success = false, Message = $"Login failed: {ex.Message}" };
        }
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("userId", user.Id.ToString()),
                new System.Security.Claims.Claim("username", user.Username),
                new System.Security.Claims.Claim("email", user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"] ?? "60")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task LogoutAsync(Guid userId)
    {
        // Implement logout logic (e.g., blacklist token, clear session)
        return Task.CompletedTask;
    }
}
