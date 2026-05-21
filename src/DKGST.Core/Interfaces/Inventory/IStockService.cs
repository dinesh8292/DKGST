using DKGST.Core.Models.Inventory;

namespace DKGST.Core.Interfaces.Inventory;

public interface IStockService
{
    Task<Stock?> GetStockAsync(Guid productId, Guid companyId);
    Task<List<Stock>> GetStocksByCompanyAsync(Guid companyId);
    Task<bool> UpdateStockAsync(Stock stock);
    Task<bool> CreateStockTransferAsync(StockTransfer transfer);
    Task<bool> ReceiveStockTransferAsync(Guid transferId);
    Task<bool> CreateStockAdjustmentAsync(StockAdjustment adjustment);
    Task<bool> ApproveStockAdjustmentAsync(Guid adjustmentId);
    Task<List<StockTransfer>> GetStockTransfersByCompanyAsync(Guid companyId);
}
