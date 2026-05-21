using DKGST.Core.Interfaces.Accounting;
using DKGST.Core.Models.Accounting;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services.Accounting;

public class GstReportService : IGstReportService
{
    private readonly CompanyDbContext _context;

    public GstReportService(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<List<GstReport>> GetGstReportsByCompanyAsync(Guid companyId)
    {
        return await _context.Set<GstReport>()
            .Where(r => r.CompanyId == companyId)
            .Include(r => r.Transactions)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }

    public async Task<GstReport?> GetGstReportByIdAsync(Guid reportId)
    {
        return await _context.Set<GstReport>()
            .Include(r => r.Transactions)
            .FirstOrDefaultAsync(r => r.Id == reportId);
    }

    public async Task<GstReport> CreateGstReportAsync(GstReport report)
    {
        _context.Set<GstReport>().Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<GstReport> GenerateGstReportAsync(Guid companyId, DateTime startDate, DateTime endDate, string period)
    {
        var report = new GstReport
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            StartDate = startDate,
            EndDate = endDate,
            ReportPeriod = period,
            CreatedAt = DateTime.UtcNow
        };

        // Calculate GST totals from invoices
        var invoices = await _context.Set<Invoice>()
            .Where(i => i.CompanyId == companyId 
                && i.InvoiceDate >= startDate 
                && i.InvoiceDate <= endDate
                && i.Status != "Cancelled")
            .ToListAsync();

        report.TotalSales = invoices.Sum(i => i.TotalAmount);
        report.OutputGst = invoices.Sum(i => i.TaxAmount);

        // Add report
        _context.Set<GstReport>().Add(report);
        await _context.SaveChangesAsync();

        return report;
    }

    public async Task<bool> SubmitGstReportAsync(Guid reportId)
    {
        var report = await GetGstReportByIdAsync(reportId);
        if (report == null) return false;

        report.Status = "Submitted";
        report.SubmittedAt = DateTime.UtcNow;
        _context.Set<GstReport>().Update(report);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<GstTransaction>> GetGstTransactionsAsync(Guid reportId)
    {
        return await _context.Set<GstTransaction>()
            .Where(t => t.GstReportId == reportId)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync();
    }
}
