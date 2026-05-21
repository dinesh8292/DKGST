using DKGST.Core.Models;

namespace DKGST.Core.Interfaces;

public interface IPermissionService
{
    Task<List<Permission>> GetPermissionsByRoleAsync(Guid roleId);
    Task<bool> HasPermissionAsync(Guid userId, string permissionCode);
    Task<List<Permission>> GetAllPermissionsAsync();
    Task<Permission> CreatePermissionAsync(Permission permission);
}
