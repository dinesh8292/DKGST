using DKGST.Core.Interfaces.Accounting;
using DKGST.Core.Models.Accounting;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services.Accounting;

public class JournalEntryService : IJournalEntryService
{
    private readonly CompanyDbContext _context;

    public JournalEntryService(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<List<JournalEntry>> GetJournalEntriesByCompanyAsync(Guid companyId)
    {
        return await _context.Set<JournalEntry>()
            .Where(je => je.CompanyId == companyId)
            .Include(je => je.JournalLines)
            .OrderByDescending(je => je.EntryDate)
            .ToListAsync();
    }

    public async Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId)
    {
        return await _context.Set<JournalEntry>()
            .Include(je => je.JournalLines)
            .FirstOrDefaultAsync(je => je.Id == entryId);
    }

    public async Task<JournalEntry> CreateJournalEntryAsync(JournalEntry entry)
    {
        entry.EntryNumber = GenerateEntryNumber();
        _context.Set<JournalEntry>().Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> PostJournalEntryAsync(Guid entryId)
    {
        var entry = await GetJournalEntryByIdAsync(entryId);
        if (entry == null) return false;

        // Validate debit = credit
        var totalDebit = entry.JournalLines.Sum(l => l.DebitAmount);
        var totalCredit = entry.JournalLines.Sum(l => l.CreditAmount);
        if (totalDebit != totalCredit) return false;

        entry.Status = "Posted";
        _context.Set<JournalEntry>().Update(entry);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ReverseJournalEntryAsync(Guid entryId)
    {
        var entry = await GetJournalEntryByIdAsync(entryId);
        if (entry == null) return false;

        entry.Status = "Reversed";
        _context.Set<JournalEntry>().Update(entry);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Account>> GetAccountsAsync(Guid companyId)
    {
        return await _context.Set<Account>()
            .Where(a => a.CompanyId == companyId && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountByCodeAsync(Guid companyId, string accountCode)
    {
        return await _context.Set<Account>()
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountCode == accountCode);
    }

    private string GenerateEntryNumber()
    {
        return $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
