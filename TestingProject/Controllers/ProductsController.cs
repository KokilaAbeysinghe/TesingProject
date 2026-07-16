using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
            return NotFound("Product not found!");
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AddProduct(CreateProductDTO productDTO)
    {
        await _productService.AddProduct(productDTO);
        return Ok(new { message = "Product added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, CreateProductDTO productDTO)
    {
        await _productService.UpdateProduct(id, productDTO);
        return Ok(new { message = "Product updated successfully!" });
    }

    [HttpPost("{id}/adjust-stock")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AdjustStock(int id, AdjustStockDTO adjustStockDTO)
    {
        await _productService.AdjustStock(id, adjustStockDTO);
        return Ok(new { message = "Stock adjusted successfully!" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProduct(id);
        return Ok(new { message = "Product deleted successfully!" });
    }
}