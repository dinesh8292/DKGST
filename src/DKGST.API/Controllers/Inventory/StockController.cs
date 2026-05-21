using DKGST.Core.Interfaces.Inventory;
using DKGST.Core.Models.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers.Inventory;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetStocks(Guid companyId)
    {
        var stocks = await _stockService.GetStocksByCompanyAsync(companyId);
        return Ok(stocks);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> CreateStockTransfer([FromBody] StockTransfer transfer)
    {
        var result = await _stockService.CreateStockTransferAsync(transfer);
        if (!result)
            return BadRequest(new { message = "Failed to create stock transfer" });
        return Ok(new { message = "Stock transfer created successfully" });
    }

    [HttpPost("transfer/{transferId}/receive")]
    public async Task<IActionResult> ReceiveStockTransfer(Guid transferId)
    {
        var result = await _stockService.ReceiveStockTransferAsync(transferId);
        if (!result)
            return NotFound(new { message = "Transfer not found" });
        return Ok(new { message = "Stock transfer received" });
    }

    [HttpPost("adjustment")]
    public async Task<IActionResult> CreateStockAdjustment([FromBody] StockAdjustment adjustment)
    {
        var result = await _stockService.CreateStockAdjustmentAsync(adjustment);
        if (!result)
            return BadRequest(new { message = "Failed to create stock adjustment" });
        return Ok(new { message = "Stock adjustment created successfully" });
    }
}
