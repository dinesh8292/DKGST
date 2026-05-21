namespace DKGST.Core.Models;

public class Menu
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int Order { get; set; }
    public Guid? ParentId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string RequiredPermission { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public bool IsActive { get; set; } = true;
    
    public Company? Company { get; set; }
    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
}
