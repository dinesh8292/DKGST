using DKGST.Core.Models.Accounting;

namespace DKGST.Core.Interfaces.Accounting;

public interface IGstReportService
{
    Task<List<GstReport>> GetGstReportsByCompanyAsync(Guid companyId);
    Task<GstReport?> GetGstReportByIdAsync(Guid reportId);
    Task<GstReport> CreateGstReportAsync(GstReport report);
    Task<GstReport> GenerateGstReportAsync(Guid companyId, DateTime startDate, DateTime endDate, string period);
    Task<bool> SubmitGstReportAsync(Guid reportId);
    Task<List<GstTransaction>> GetGstTransactionsAsync(Guid reportId);
}
