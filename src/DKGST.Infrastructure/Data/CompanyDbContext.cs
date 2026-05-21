using DKGST.Core.Models;
using DKGST.Core.Models.Accounting;
using DKGST.Core.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Infrastructure.Data;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

    // Accounting Module
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;
    public DbSet<InvoicePayment> InvoicePayments { get; set; } = null!;
    public DbSet<GstReport> GstReports { get; set; } = null!;
    public DbSet<GstTransaction> GstTransactions { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<JournalLine> JournalLines { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;

    // Inventory Module
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Stock> Stocks { get; set; } = null!;
    public DbSet<StockTransfer> StockTransfers { get; set; } = null!;
    public DbSet<StockTransferItem> StockTransferItems { get; set; } = null!;
    public DbSet<StockAdjustment> StockAdjustments { get; set; } = null!;
    public DbSet<StockAdjustmentItem> StockAdjustmentItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Invoice Configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.HasMany(e => e.InvoiceItems)
                .WithOne(ei => ei.Invoice)
                .HasForeignKey(ei => ei.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // InvoiceItem Configuration
        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(18, 2);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
        });

        // GstReport Configuration
        modelBuilder.Entity<GstReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalSales).HasPrecision(18, 2);
            entity.Property(e => e.TotalPurchases).HasPrecision(18, 2);
            entity.Property(e => e.OutputGst).HasPrecision(18, 2);
            entity.Property(e => e.InputGst).HasPrecision(18, 2);
            entity.Property(e => e.NetGst).HasPrecision(18, 2);
        });

        // JournalEntry Configuration
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntryNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.EntryNumber).IsUnique();
        });

        // Product Configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ProductCode).IsUnique();
            entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
            entity.Property(e => e.SellingPrice).HasPrecision(18, 2);
            entity.Property(e => e.TaxRate).HasPrecision(5, 2);
        });

        // Stock Configuration
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.QuantityOnHand).HasPrecision(18, 2);
            entity.Property(e => e.QuantityReserved).HasPrecision(18, 2);
            entity.Property(e => e.QuantityAvailable).HasPrecision(18, 2);
        });

        // StockTransfer Configuration
        modelBuilder.Entity<StockTransfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransferNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.TransferNumber).IsUnique();
        });

        // StockAdjustment Configuration
        modelBuilder.Entity<StockAdjustment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AdjustmentNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.AdjustmentNumber).IsUnique();
        });
    }
}
