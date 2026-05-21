using DKGST.Core.Models.Accounting;

namespace DKGST.Core.Interfaces.Accounting;

public interface IInvoiceService
{
    Task<List<Invoice>> GetInvoicesByCompanyAsync(Guid companyId);
    Task<Invoice?> GetInvoiceByIdAsync(Guid invoiceId);
    Task<Invoice> CreateInvoiceAsync(Invoice invoice);
    Task<bool> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> DeleteInvoiceAsync(Guid invoiceId);
    Task<bool> SubmitInvoiceAsync(Guid invoiceId);
    Task<List<Invoice>> GetPendingInvoicesAsync(Guid companyId);
}
