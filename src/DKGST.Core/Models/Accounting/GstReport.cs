namespace DKGST.Core.Models.Accounting;

public class GstReport
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal OutputGst { get; set; }
    public decimal InputGst { get; set; }
    public decimal NetGst { get; set; }
    public string ReportPeriod { get; set; } = string.Empty; // Monthly, Quarterly, Annual
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    
    public ICollection<GstTransaction> Transactions { get; set; } = new List<GstTransaction>();
}

public class GstTransaction
{
    public Guid Id { get; set; }
    public Guid GstReportId { get; set; }
    public string TransactionType { get; set; } = string.Empty; // Sale, Purchase
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string PartyName { get; set; } = string.Empty;
    public string PartyGst { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal GstAmount { get; set; }
    public string GstType { get; set; } = string.Empty; // IGST, CGST, SGST
    
    public GstReport? GstReport { get; set; }
}
