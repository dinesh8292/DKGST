using DKGST.Core.Models;
using DKGST.Core.Models.Accounting;
using DKGST.Core.Models.Inventory;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedMasterDataAsync(MasterDbContext context)
    {
        try
        {
            // Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new() { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator with full access", IsSystemRole = true, IsActive = true },
                    new() { Id = Guid.NewGuid(), Name = "Accountant", Description = "Accounting module access", IsSystemRole = false, IsActive = true },
                    new() { Id = Guid.NewGuid(), Name = "Inventory Manager", Description = "Inventory module access", IsSystemRole = false, IsActive = true },
                    new() { Id = Guid.NewGuid(), Name = "User", Description = "Limited access user", IsSystemRole = false, IsActive = true }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // Seed Permissions
            if (!await context.Permissions.AnyAsync())
            {
                var permissions = new List<Permission>
                {
                    // Accounting Permissions
                    new() { Id = Guid.NewGuid(), Code = "INVOICE_VIEW", Name = "View Invoices", Module = "Accounting", Description = "View invoice list", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "INVOICE_CREATE", Name = "Create Invoice", Module = "Accounting", Description = "Create new invoice", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "INVOICE_EDIT", Name = "Edit Invoice", Module = "Accounting", Description = "Edit invoice", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "INVOICE_DELETE", Name = "Delete Invoice", Module = "Accounting", Description = "Delete invoice", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "GST_REPORT_VIEW", Name = "View GST Reports", Module = "Accounting", Description = "View GST reports", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "GST_REPORT_GENERATE", Name = "Generate GST Report", Module = "Accounting", Description = "Generate new GST report", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "JOURNAL_VIEW", Name = "View Journal Entries", Module = "Accounting", Description = "View journal entries", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "JOURNAL_CREATE", Name = "Create Journal Entry", Module = "Accounting", Description = "Create journal entry", IsActive = true },
                    
                    // Inventory Permissions
                    new() { Id = Guid.NewGuid(), Code = "PRODUCT_VIEW", Name = "View Products", Module = "Inventory", Description = "View product list", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "PRODUCT_CREATE", Name = "Create Product", Module = "Inventory", Description = "Create new product", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "PRODUCT_EDIT", Name = "Edit Product", Module = "Inventory", Description = "Edit product", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "STOCK_VIEW", Name = "View Stock", Module = "Inventory", Description = "View stock levels", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "STOCK_TRANSFER", Name = "Transfer Stock", Module = "Inventory", Description = "Create stock transfer", IsActive = true },
                    new() { Id = Guid.NewGuid(), Code = "STOCK_ADJUST", Name = "Adjust Stock", Module = "Inventory", Description = "Adjust stock quantity", IsActive = true }
                };
                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }

            // Seed Users
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Email = "admin@dkgst.com",
                        FirstName = "Admin",
                        LastName = "User",
                        PasswordHash = HashPassword("admin123"),
                        IsActive = true,
                        PreferredLanguage = "en",
                        CreatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Username = "accountant",
                        Email = "accountant@dkgst.com",
                        FirstName = "John",
                        LastName = "Accountant",
                        PasswordHash = HashPassword("accountant123"),
                        IsActive = true,
                        PreferredLanguage = "en",
                        CreatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Username = "inventory",
                        Email = "inventory@dkgst.com",
                        FirstName = "Jane",
                        LastName = "Manager",
                        PasswordHash = HashPassword("inventory123"),
                        IsActive = true,
                        PreferredLanguage = "en",
                        CreatedAt = DateTime.UtcNow
                    }
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Seed Companies
            if (!await context.Companies.AnyAsync())
            {
                var companies = new List<Company>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Code = "COMP001",
                        Name = "ABC Trading India Private Limited",
                        Address = "123 Business Street",
                        City = "Delhi",
                        State = "Delhi",
                        ZipCode = "110001",
                        GstNumber = "07AABCS1234H1Z0",
                        Pan = "AABCS1234H",
                        ContactPerson = "Rajesh Kumar",
                        PhoneNumber = "9876543210",
                        Email = "info@abctrading.com",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "admin"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Code = "COMP002",
                        Name = "XYZ Enterprises Limited",
                        Address = "456 Commerce Road",
                        City = "Mumbai",
                        State = "Maharashtra",
                        ZipCode = "400001",
                        GstNumber = "27AABCS5678H2Z0",
                        Pan = "AABCS5678H",
                        ContactPerson = "Priya Singh",
                        PhoneNumber = "9123456789",
                        Email = "contact@xyzenterprises.com",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "admin"
                    }
                };
                context.Companies.AddRange(companies);
                await context.SaveChangesAsync();
            }

            // Link Users to Companies
            if (!await context.UserCompanies.AnyAsync())
            {
                var admin = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                var accountant = await context.Users.FirstOrDefaultAsync(u => u.Username == "accountant");
                var inventory = await context.Users.FirstOrDefaultAsync(u => u.Username == "inventory");
                var comp1 = await context.Companies.FirstOrDefaultAsync(c => c.Code == "COMP001");
                var comp2 = await context.Companies.FirstOrDefaultAsync(c => c.Code == "COMP002");

                if (admin != null && comp1 != null && comp2 != null)
                {
                    context.UserCompanies.AddRange(
                        new UserCompany { Id = Guid.NewGuid(), UserId = admin.Id, CompanyId = comp1.Id, IsDefault = true },
                        new UserCompany { Id = Guid.NewGuid(), UserId = admin.Id, CompanyId = comp2.Id, IsDefault = false }
                    );
                }

                if (accountant != null && comp1 != null)
                {
                    context.UserCompanies.Add(
                        new UserCompany { Id = Guid.NewGuid(), UserId = accountant.Id, CompanyId = comp1.Id, IsDefault = true }
                    );
                }

                if (inventory != null && comp1 != null)
                {
                    context.UserCompanies.Add(
                        new UserCompany { Id = Guid.NewGuid(), UserId = inventory.Id, CompanyId = comp1.Id, IsDefault = true }
                    );
                }

                await context.SaveChangesAsync();
            }

            // Link Roles to Users
            if (!await context.UserRoles.AnyAsync())
            {
                var admin = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                var accountant = await context.Users.FirstOrDefaultAsync(u => u.Username == "accountant");
                var inventory = await context.Users.FirstOrDefaultAsync(u => u.Username == "inventory");
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var accountantRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Accountant");
                var inventoryRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Inventory Manager");

                if (admin != null && adminRole != null)
                    context.UserRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = admin.Id, RoleId = adminRole.Id });

                if (accountant != null && accountantRole != null)
                    context.UserRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = accountant.Id, RoleId = accountantRole.Id });

                if (inventory != null && inventoryRole != null)
                    context.UserRoles.Add(new UserRole { Id = Guid.NewGuid(), UserId = inventory.Id, RoleId = inventoryRole.Id });

                await context.SaveChangesAsync();
            }

            // Link Permissions to Roles
            if (!await context.RolePermissions.AnyAsync())
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var accountantRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Accountant");
                var inventoryRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Inventory Manager");
                var permissions = await context.Permissions.ToListAsync();

                if (adminRole != null)
                {
                    foreach (var perm in permissions)
                    {
                        context.RolePermissions.Add(new RolePermission { Id = Guid.NewGuid(), RoleId = adminRole.Id, PermissionId = perm.Id });
                    }
                }

                if (accountantRole != null)
                {
                    var accountingPerms = permissions.Where(p => p.Module == "Accounting").ToList();
                    foreach (var perm in accountingPerms)
                    {
                        context.RolePermissions.Add(new RolePermission { Id = Guid.NewGuid(), RoleId = accountantRole.Id, PermissionId = perm.Id });
                    }
                }

                if (inventoryRole != null)
                {
                    var inventoryPerms = permissions.Where(p => p.Module == "Inventory").ToList();
                    foreach (var perm in inventoryPerms)
                    {
                        context.RolePermissions.Add(new RolePermission { Id = Guid.NewGuid(), RoleId = inventoryRole.Id, PermissionId = perm.Id });
                    }
                }

                await context.SaveChangesAsync();
            }

            // Seed Menus
            if (!await context.Menus.AnyAsync())
            {
                var comp1 = await context.Companies.FirstOrDefaultAsync(c => c.Code == "COMP001");
                if (comp1 != null)
                {
                    var menus = new List<Menu>
                    {
                        // Accounting Module
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Accounting", Route = "#", Icon = "bi bi-calculator", Order = 1, Module = "Accounting", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Invoices", Route = "/accounting/invoices", Icon = "bi bi-file-earmark-text", Order = 1, ParentId = menus.FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Accounting", RequiredPermission = "INVOICE_VIEW", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "GST Reports", Route = "/accounting/gst-reports", Icon = "bi bi-file-pdf", Order = 2, ParentId = menus.FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Accounting", RequiredPermission = "GST_REPORT_VIEW", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Journal", Route = "/accounting/journal", Icon = "bi bi-journal-text", Order = 3, ParentId = menus.FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Accounting", RequiredPermission = "JOURNAL_VIEW", IsVisible = true, IsActive = true },

                        // Inventory Module
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Inventory", Route = "#", Icon = "bi bi-boxes", Order = 2, Module = "Inventory", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Products", Route = "/inventory/products", Icon = "bi bi-box", Order = 1, ParentId = menus.Skip(4).FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Inventory", RequiredPermission = "PRODUCT_VIEW", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Stock", Route = "/inventory/stock", Icon = "bi bi-graph-up", Order = 2, ParentId = menus.Skip(4).FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Inventory", RequiredPermission = "STOCK_VIEW", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Transfers", Route = "/inventory/transfers", Icon = "bi bi-arrow-left-right", Order = 3, ParentId = menus.Skip(4).FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Inventory", RequiredPermission = "STOCK_TRANSFER", IsVisible = true, IsActive = true },
                        new() { Id = Guid.NewGuid(), CompanyId = comp1.Id, Title = "Adjustments", Route = "/inventory/adjustments", Icon = "bi bi-pencil-square", Order = 4, ParentId = menus.Skip(4).FirstOrDefault()?.Id ?? Guid.NewGuid(), Module = "Inventory", RequiredPermission = "STOCK_ADJUST", IsVisible = true, IsActive = true }
                    };
                    context.Menus.AddRange(menus);
                    await context.SaveChangesAsync();
                }
            }

            Console.WriteLine("✅ Master data seeded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding data: {ex.Message}");
        }
    }

    public static async Task SeedCompanyDataAsync(CompanyDbContext context, Guid companyId)
    {
        try
        {
            // Seed Accounts
            if (!await context.Set<Account>().AnyAsync(a => a.CompanyId == companyId))
            {
                var accounts = new List<Account>
                {
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "1000", AccountName = "Cash", AccountType = "Asset", OpeningBalance = 100000, CurrentBalance = 100000, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "1010", AccountName = "Bank Account", AccountType = "Asset", OpeningBalance = 500000, CurrentBalance = 500000, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "2000", AccountName = "Accounts Payable", AccountType = "Liability", OpeningBalance = 50000, CurrentBalance = 50000, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "3000", AccountName = "Capital", AccountType = "Equity", OpeningBalance = 1000000, CurrentBalance = 1000000, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "4000", AccountName = "Sales Revenue", AccountType = "Revenue", OpeningBalance = 0, CurrentBalance = 0, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, AccountCode = "5000", AccountName = "Cost of Goods Sold", AccountType = "Expense", OpeningBalance = 0, CurrentBalance = 0, IsActive = true }
                };
                context.Set<Account>().AddRange(accounts);
                await context.SaveChangesAsync();
            }

            // Seed Products
            if (!await context.Set<Product>().AnyAsync(p => p.CompanyId == companyId))
            {
                var products = new List<Product>
                {
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, ProductName = "Laptop", Category = "Electronics", Unit = "Piece", PurchasePrice = 40000, SellingPrice = 50000, ReorderLevel = 5, Hsn = "8471.30", TaxRate = 18, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, ProductName = "Office Chair", Category = "Furniture", Unit = "Piece", PurchasePrice = 5000, SellingPrice = 7000, ReorderLevel = 10, Hsn = "9401.80", TaxRate = 18, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, ProductName = "Desk Lamp", Category = "Lighting", Unit = "Piece", PurchasePrice = 800, SellingPrice = 1200, ReorderLevel = 20, Hsn = "9405.40", TaxRate = 18, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, ProductName = "Notebook Set", Category = "Stationery", Unit = "Box", PurchasePrice = 200, SellingPrice = 350, ReorderLevel = 50, Hsn = "4820.10", TaxRate = 5, IsActive = true },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, ProductName = "Printer", Category = "Electronics", Unit = "Piece", PurchasePrice = 15000, SellingPrice = 20000, ReorderLevel = 3, Hsn = "8443.31", TaxRate = 18, IsActive = true }
                };
                context.Set<Product>().AddRange(products);
                await context.SaveChangesAsync();
            }

            // Seed Stock
            if (!await context.Set<Stock>().AnyAsync(s => s.CompanyId == companyId))
            {
                var products = await context.Set<Product>().Where(p => p.CompanyId == companyId).ToListAsync();
                var stocks = new List<Stock>();
                foreach (var product in products)
                {
                    stocks.Add(new Stock
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        CompanyId = companyId,
                        WarehouseLocation = "Main Warehouse",
                        QuantityOnHand = 100,
                        QuantityReserved = 10,
                        QuantityAvailable = 90,
                        LastUpdated = DateTime.UtcNow
                    });
                }
                context.Set<Stock>().AddRange(stocks);
                await context.SaveChangesAsync();
            }

            // Seed Sample Invoices
            if (!await context.Set<Invoice>().AnyAsync(i => i.CompanyId == companyId))
            {
                var invoices = new List<Invoice>
                {
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, InvoiceDate = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(30), CustomerName = "Tech Solutions Inc", CustomerGst = "07AABCS1234H1Z0", CustomerAddress = "Mumbai, India", SubTotal = 100000, TaxAmount = 18000, TotalAmount = 118000, Status = "Submitted", CreatedAt = DateTime.UtcNow, CreatedBy = Guid.Empty },
                    new() { Id = Guid.NewGuid(), CompanyId = companyId, InvoiceDate = DateTime.UtcNow.AddDays(-5), DueDate = DateTime.UtcNow.AddDays(25), CustomerName = "Global Enterprises", CustomerGst = "27AABCS5678H2Z0", CustomerAddress = "Delhi, India", SubTotal = 50000, TaxAmount = 9000, TotalAmount = 59000, Status = "Draft", CreatedAt = DateTime.UtcNow, CreatedBy = Guid.Empty }
                };
                context.Set<Invoice>().AddRange(invoices);
                await context.SaveChangesAsync();
            }

            Console.WriteLine($"✅ Company data seeded successfully for company: {companyId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error seeding company data: {ex.Message}");
        }
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(hashedBytes);
        }
    }
}
