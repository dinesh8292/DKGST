namespace DKGST.Core.Models.Inventory;

public class StockTransfer
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string TransferNumber { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string FromWarehouse { get; set; } = string.Empty;
    public string ToWarehouse { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, In Transit, Received, Cancelled
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public ICollection<StockTransferItem> TransferItems { get; set; } = new List<StockTransferItem>();
}

public class StockTransferItem
{
    public Guid Id { get; set; }
    public Guid StockTransferId { get; set; }
    public Guid ProductId { get; set; }
    public decimal QuantityRequested { get; set; }
    public decimal QuantityReceived { get; set; }
    public string Notes { get; set; } = string.Empty;
    
    public StockTransfer? StockTransfer { get; set; }
}

public class StockAdjustment
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string AdjustmentNumber { get; set; } = string.Empty;
    public DateTime AdjustmentDate { get; set; }
    public string Reason { get; set; } = string.Empty; // Damage, Expiry, Theft, Recount, etc.
    public string Status { get; set; } = "Draft"; // Draft, Approved, Reversed
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    
    public ICollection<StockAdjustmentItem> AdjustmentItems { get; set; } = new List<StockAdjustmentItem>();
}

public class StockAdjustmentItem
{
    public Guid Id { get; set; }
    public Guid StockAdjustmentId { get; set; }
    public Guid ProductId { get; set; }
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    
    public StockAdjustment? StockAdjustment { get; set; }
}
