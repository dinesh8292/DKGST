using DKGST.Core.Interfaces;
using DKGST.Core.Models;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services;

public class CompanyService : ICompanyService
{
    private readonly MasterDbContext _context;

    public CompanyService(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompanyDto>> GetUserCompaniesAsync(Guid userId)
    {
        return await _context.UserCompanies
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Company)
            .Select(uc => new CompanyDto
            {
                Id = uc.Company.Id,
                Code = uc.Company.Code,
                Name = uc.Company.Name,
                GstNumber = uc.Company.GstNumber,
                IsDefault = uc.IsDefault
            })
            .ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid companyId)
    {
        return await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
    }

    public async Task<Company> CreateCompanyAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<bool> UpdateCompanyAsync(Company company)
    {
        _context.Companies.Update(company);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteCompanyAsync(Guid companyId)
    {
        var company = await GetCompanyByIdAsync(companyId);
        if (company == null) return false;

        _context.Companies.Remove(company);
        return await _context.SaveChangesAsync() > 0;
    }
}
