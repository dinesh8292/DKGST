namespace DKGST.Core.Models.Accounting;

public class JournalEntry
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft"; // Draft, Posted, Reversed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    
    public ICollection<JournalLine> JournalLines { get; set; } = new List<JournalLine>();
}

public class JournalLine
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public JournalEntry? JournalEntry { get; set; }
}

public class Account
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty; // Asset, Liability, Equity, Revenue, Expense
    public decimal OpeningBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
