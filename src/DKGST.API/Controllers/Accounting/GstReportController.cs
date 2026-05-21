using DKGST.Core.Interfaces.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers.Accounting;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GstReportController : ControllerBase
{
    private readonly IGstReportService _gstReportService;

    public GstReportController(IGstReportService gstReportService)
    {
        _gstReportService = gstReportService;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetGstReports(Guid companyId)
    {
        var reports = await _gstReportService.GetGstReportsByCompanyAsync(companyId);
        return Ok(reports);
    }

    [HttpGet("detail/{reportId}")]
    public async Task<IActionResult> GetGstReport(Guid reportId)
    {
        var report = await _gstReportService.GetGstReportByIdAsync(reportId);
        if (report == null)
            return NotFound(new { message = "Report not found" });
        return Ok(report);
    }

    [HttpPost("{companyId}/generate")]
    public async Task<IActionResult> GenerateGstReport(Guid companyId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string period)
    {
        var report = await _gstReportService.GenerateGstReportAsync(companyId, startDate, endDate, period);
        return CreatedAtAction(nameof(GetGstReport), new { reportId = report.Id }, report);
    }

    [HttpPost("{reportId}/submit")]
    public async Task<IActionResult> SubmitGstReport(Guid reportId)
    {
        var result = await _gstReportService.SubmitGstReportAsync(reportId);
        if (!result)
            return NotFound(new { message = "Report not found" });
        return Ok(new { message = "Report submitted successfully" });
    }
}
