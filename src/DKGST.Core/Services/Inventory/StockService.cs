using DKGST.Core.Interfaces.Inventory;
using DKGST.Core.Models.Inventory;
using DKGST.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DKGST.Core.Services.Inventory;

public class StockService : IStockService
{
    private readonly CompanyDbContext _context;

    public StockService(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetStockAsync(Guid productId, Guid companyId)
    {
        return await _context.Set<Stock>()
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.CompanyId == companyId);
    }

    public async Task<List<Stock>> GetStocksByCompanyAsync(Guid companyId)
    {
        return await _context.Set<Stock>()
            .Where(s => s.CompanyId == companyId)
            .Include(s => s.Product)
            .OrderBy(s => s.WarehouseLocation)
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(Stock stock)
    {
        stock.QuantityAvailable = stock.QuantityOnHand - stock.QuantityReserved;
        stock.LastUpdated = DateTime.UtcNow;
        _context.Set<Stock>().Update(stock);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateStockTransferAsync(StockTransfer transfer)
    {
        transfer.TransferNumber = GenerateTransferNumber();
        _context.Set<StockTransfer>().Add(transfer);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ReceiveStockTransferAsync(Guid transferId)
    {
        var transfer = await _context.Set<StockTransfer>()
            .Include(t => t.TransferItems)
            .FirstOrDefaultAsync(t => t.Id == transferId);

        if (transfer == null) return false;

        transfer.Status = "Received";
        transfer.CompletedAt = DateTime.UtcNow;
        _context.Set<StockTransfer>().Update(transfer);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateStockAdjustmentAsync(StockAdjustment adjustment)
    {
        adjustment.AdjustmentNumber = GenerateAdjustmentNumber();
        _context.Set<StockAdjustment>().Add(adjustment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ApproveStockAdjustmentAsync(Guid adjustmentId)
    {
        var adjustment = await _context.Set<StockAdjustment>()
            .Include(a => a.AdjustmentItems)
            .FirstOrDefaultAsync(a => a.Id == adjustmentId);

        if (adjustment == null) return false;

        adjustment.Status = "Approved";
        _context.Set<StockAdjustment>().Update(adjustment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<StockTransfer>> GetStockTransfersByCompanyAsync(Guid companyId)
    {
        return await _context.Set<StockTransfer>()
            .Where(t => t.CompanyId == companyId)
            .Include(t => t.TransferItems)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    private string GenerateTransferNumber()
    {
        return $"STR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
    }

    private string GenerateAdjustmentNumber()
    {
        return $"ADJ-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
    }
}
