using DKGST.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCompanies()
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
        var companies = await _companyService.GetUserCompaniesAsync(userId);
        return Ok(companies);
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompany(Guid companyId)
    {
        var company = await _companyService.GetCompanyByIdAsync(companyId);
        if (company == null)
            return NotFound(new { message = "Company not found" });

        return Ok(company);
    }
}
