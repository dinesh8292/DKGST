using Microsoft.EntityFrameworkCore;

namespace DKGST.Infrastructure.Data;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

    // Add company-specific DbSets here
    // Example: Invoices, Transactions, Products, etc.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure company-specific entities
    }
}
