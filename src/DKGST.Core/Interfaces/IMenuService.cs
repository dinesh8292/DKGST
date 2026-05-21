using DKGST.Core.Models;

namespace DKGST.Core.Interfaces;

public interface IMenuService
{
    Task<List<Menu>> GetMenuByCompanyAsync(Guid companyId, Guid userId);
    Task<List<Menu>> GetMenuByRoleAsync(Guid companyId, Guid roleId);
    Task<Menu> CreateMenuAsync(Menu menu);
    Task<bool> UpdateMenuAsync(Menu menu);
    Task<bool> DeleteMenuAsync(Guid menuId);
}
