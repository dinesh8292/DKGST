using DKGST.Core.Models.Accounting;

namespace DKGST.Core.Interfaces.Accounting;

public interface IJournalEntryService
{
    Task<List<JournalEntry>> GetJournalEntriesByCompanyAsync(Guid companyId);
    Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId);
    Task<JournalEntry> CreateJournalEntryAsync(JournalEntry entry);
    Task<bool> PostJournalEntryAsync(Guid entryId);
    Task<bool> ReverseJournalEntryAsync(Guid entryId);
    Task<List<Account>> GetAccountsAsync(Guid companyId);
    Task<Account?> GetAccountByCodeAsync(Guid companyId, string accountCode);
}
