using DKGST.Core.Interfaces.Accounting;
using DKGST.Core.Models.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers.Accounting;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetInvoices(Guid companyId)
    {
        var invoices = await _invoiceService.GetInvoicesByCompanyAsync(companyId);
        return Ok(invoices);
    }

    [HttpGet("{companyId}/pending")]
    public async Task<IActionResult> GetPendingInvoices(Guid companyId)
    {
        var invoices = await _invoiceService.GetPendingInvoicesAsync(companyId);
        return Ok(invoices);
    }

    [HttpGet("detail/{invoiceId}")]
    public async Task<IActionResult> GetInvoice(Guid invoiceId)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if (invoice == null)
            return NotFound(new { message = "Invoice not found" });
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
    {
        var result = await _invoiceService.CreateInvoiceAsync(invoice);
        return CreatedAtAction(nameof(GetInvoice), new { invoiceId = result.Id }, result);
    }

    [HttpPut("{invoiceId}")]
    public async Task<IActionResult> UpdateInvoice(Guid invoiceId, [FromBody] Invoice invoice)
    {
        invoice.Id = invoiceId;
        var result = await _invoiceService.UpdateInvoiceAsync(invoice);
        if (!result)
            return NotFound(new { message = "Invoice not found" });
        return Ok(new { message = "Invoice updated successfully" });
    }

    [HttpPost("{invoiceId}/submit")]
    public async Task<IActionResult> SubmitInvoice(Guid invoiceId)
    {
        var result = await _invoiceService.SubmitInvoiceAsync(invoiceId);
        if (!result)
            return NotFound(new { message = "Invoice not found" });
        return Ok(new { message = "Invoice submitted successfully" });
    }
}
