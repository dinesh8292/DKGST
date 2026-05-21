using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services;

public class MenuService : IMenuService
{
    private readonly MasterDbContext _context;
    private readonly IPermissionService _permissionService;

    public MenuService(MasterDbContext context, IPermissionService permissionService)
    {
        _context = context;
        _permissionService = permissionService;
    }

    public async Task<List<Menu>> GetMenuByCompanyAsync(Guid companyId, Guid userId)
    {
        var menus = await _context.Menus
            .Where(m => m.CompanyId == companyId && m.ParentId == null && m.IsActive)
            .Include(m => m.Children)
            .OrderBy(m => m.Order)
            .ToListAsync();

        // Filter by user permissions
        var userMenus = new List<Menu>();
        foreach (var menu in menus)
        {
            if (string.IsNullOrEmpty(menu.RequiredPermission) || await _permissionService.HasPermissionAsync(userId, menu.RequiredPermission))
            {
                userMenus.Add(menu);
            }
        }

        return userMenus;
    }

    public async Task<List<Menu>> GetMenuByRoleAsync(Guid companyId, Guid roleId)
    {
        return await _context.Menus
            .Where(m => m.CompanyId == companyId && m.IsActive)
            .OrderBy(m => m.Order)
            .ToListAsync();
    }

    public async Task<Menu> CreateMenuAsync(Menu menu)
    {
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
        return menu;
    }

    public async Task<bool> UpdateMenuAsync(Menu menu)
    {
        _context.Menus.Update(menu);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteMenuAsync(Guid menuId)
    {
        var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == menuId);
        if (menu == null) return false;

        _context.Menus.Remove(menu);
        return await _context.SaveChangesAsync() > 0;
    }
}
