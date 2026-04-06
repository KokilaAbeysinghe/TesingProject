using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
            return NotFound("Product not found!");
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        await _productService.AddProduct(product);
        return Ok("Product added successfully!");
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        product.Id = id;
        await _productService.UpdateProduct(product);
        return Ok("Product updated successfully!");
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProduct(id);
        return Ok("Product deleted successfully!");
    }
}