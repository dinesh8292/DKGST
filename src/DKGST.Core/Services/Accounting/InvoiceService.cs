using DKGST.Core.Interfaces.Accounting;
using DKGST.Core.Models.Accounting;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services.Accounting;

public class InvoiceService : IInvoiceService
{
    private readonly CompanyDbContext _context;

    public InvoiceService(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Invoice>> GetInvoicesByCompanyAsync(Guid companyId)
    {
        return await _context.Set<Invoice>()
            .Where(i => i.CompanyId == companyId)
            .Include(i => i.InvoiceItems)
            .Include(i => i.Payments)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(Guid invoiceId)
    {
        return await _context.Set<Invoice>()
            .Include(i => i.InvoiceItems)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
    {
        invoice.InvoiceNumber = GenerateInvoiceNumber();
        _context.Set<Invoice>().Add(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<bool> UpdateInvoiceAsync(Invoice invoice)
    {
        _context.Set<Invoice>().Update(invoice);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteInvoiceAsync(Guid invoiceId)
    {
        var invoice = await GetInvoiceByIdAsync(invoiceId);
        if (invoice == null) return false;

        _context.Set<Invoice>().Remove(invoice);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> SubmitInvoiceAsync(Guid invoiceId)
    {
        var invoice = await GetInvoiceByIdAsync(invoiceId);
        if (invoice == null) return false;

        invoice.Status = "Submitted";
        return await UpdateInvoiceAsync(invoice);
    }

    public async Task<List<Invoice>> GetPendingInvoicesAsync(Guid companyId)
    {
        return await _context.Set<Invoice>()
            .Where(i => i.CompanyId == companyId && i.Status != "Paid")
            .OrderBy(i => i.DueDate)
            .ToListAsync();
    }

    private string GenerateInvoiceNumber()
    {
        return $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
