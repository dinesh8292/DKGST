namespace DKGST.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public Guid? CurrentCompanyId { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
}

public class UserRole
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    public User? User { get; set; }
    public Role? Role { get; set; }
}

public class UserCompany
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public bool IsDefault { get; set; }
    
    public User? User { get; set; }
    public Company? Company { get; set; }
}
