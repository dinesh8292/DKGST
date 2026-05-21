namespace DKGST.Core.Models.Inventory;

public class Product
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty; // Kg, Piece, Liter, etc.
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal ReorderLevel { get; set; }
    public string Hsn { get; set; } = string.Empty; // HSN Code for GST
    public decimal TaxRate { get; set; } // Tax percentage
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}

public class Stock
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public string WarehouseLocation { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal QuantityReserved { get; set; }
    public decimal QuantityAvailable { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    public Product? Product { get; set; }
}
