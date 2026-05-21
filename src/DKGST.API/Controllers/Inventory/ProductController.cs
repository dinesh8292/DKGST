using DKGST.Core.Interfaces.Inventory;
using DKGST.Core.Models.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKGST.API.Controllers.Inventory;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetProducts(Guid companyId)
    {
        var products = await _productService.GetProductsByCompanyAsync(companyId);
        return Ok(products);
    }

    [HttpGet("{companyId}/low-stock")]
    public async Task<IActionResult> GetLowStockProducts(Guid companyId)
    {
        var products = await _productService.GetLowStockProductsAsync(companyId);
        return Ok(products);
    }

    [HttpGet("detail/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            return NotFound(new { message = "Product not found" });
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var result = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { productId = result.Id }, result);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] Product product)
    {
        product.Id = productId;
        var result = await _productService.UpdateProductAsync(product);
        if (!result)
            return NotFound(new { message = "Product not found" });
        return Ok(new { message = "Product updated successfully" });
    }
}
