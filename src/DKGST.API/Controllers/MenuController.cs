using DKGST.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetMenus(Guid companyId)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
        var menus = await _menuService.GetMenuByCompanyAsync(companyId, userId);
        return Ok(menus);
    }
}
