using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services;

public class PermissionService : IPermissionService
{
    private readonly MasterDbContext _context;

    public PermissionService(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Permission>> GetPermissionsByRoleAsync(Guid roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission!)
            .ToListAsync();
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
    {
        var hasPermission = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .SelectMany(ur => ur.Role!.RolePermissions)
            .Where(rp => rp.Permission!.Code == permissionCode)
            .AnyAsync();

        return hasPermission;
    }

    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task<Permission> CreatePermissionAsync(Permission permission)
    {
        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
        return permission;
    }
}
