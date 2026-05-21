using DKGST.Core.Models;

namespace DKGST.Core.Interfaces;

public interface ICompanyService
{
    Task<List<CompanyDto>> GetUserCompaniesAsync(Guid userId);
    Task<Company?> GetCompanyByIdAsync(Guid companyId);
    Task<Company> CreateCompanyAsync(Company company);
    Task<bool> UpdateCompanyAsync(Company company);
    Task<bool> DeleteCompanyAsync(Guid companyId);
}
