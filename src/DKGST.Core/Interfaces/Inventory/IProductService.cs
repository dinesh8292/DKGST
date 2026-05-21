using DKGST.Core.Models.Inventory;

namespace DKGST.Core.Interfaces.Inventory;

public interface IProductService
{
    Task<List<Product>> GetProductsByCompanyAsync(Guid companyId);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);
    Task<List<Product>> GetLowStockProductsAsync(Guid companyId);
}
