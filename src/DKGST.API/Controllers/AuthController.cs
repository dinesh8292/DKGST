using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
        await _authService.LogoutAsync(userId);
        return Ok(new { message = "Logged out successfully" });
    }
}
