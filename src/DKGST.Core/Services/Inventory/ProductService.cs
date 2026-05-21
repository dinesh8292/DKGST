using DKGST.Core.Interfaces.Inventory;
using DKGST.Core.Models.Inventory;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services.Inventory;

public class ProductService : IProductService
{
    private readonly CompanyDbContext _context;

    public ProductService(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsByCompanyAsync(Guid companyId)
    {
        return await _context.Set<Product>()
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .Include(p => p.Stocks)
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Set<Product>()
            .Include(p => p.Stocks)
            .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.ProductCode = GenerateProductCode();
        _context.Set<Product>().Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Set<Product>().Update(product);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var product = await GetProductByIdAsync(productId);
        if (product == null) return false;

        product.IsActive = false;
        return await UpdateProductAsync(product);
    }

    public async Task<List<Product>> GetLowStockProductsAsync(Guid companyId)
    {
        return await _context.Set<Product>()
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .Include(p => p.Stocks)
            .Where(p => p.Stocks.Any(s => s.QuantityOnHand <= p.ReorderLevel))
            .ToListAsync();
    }

    private string GenerateProductCode()
    {
        return $"PRD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
    }
}
