namespace DKGST.Core.Models.Accounting;

public class Invoice
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerGst { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Paid, Cancelled
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();
}

public class InvoiceItem
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    
    public Invoice? Invoice { get; set; }
}

public class InvoicePayment
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // Cash, Check, Bank Transfer, etc.
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Invoice? Invoice { get; set; }
}
