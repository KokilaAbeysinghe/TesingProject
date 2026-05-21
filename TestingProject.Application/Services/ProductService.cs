using TestingProject.Application.DTOs;
using TestingProject.Domain.Entities;
using TestingProject.Application.Interfaces;

namespace TestingProject.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDTO>> GetAllProducts()
    {
        var products = await _productRepository.GetAllProducts();
        return products.Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            ProductCategoryId = p.ProductCategoryId ?? 0,
            CategoryName = p.ProductCategory?.Name ?? string.Empty,
            Price = p.Price,
            Stock = p.Stock
        }).ToList();
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await _productRepository.GetProductById(id);

        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found!");

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            ProductCategoryId = product.ProductCategoryId ?? 0,
            CategoryName = product.ProductCategory?.Name ?? string.Empty,
            Price = product.Price,
            Stock = product.Stock
        };
   
    }

    public async Task AddProduct(CreateProductDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            ProductCategoryId = productDTO.ProductCategoryId,
            Price = productDTO.Price,
            Stock = productDTO.Stock
        };
        await _productRepository.AddProduct(product);
    }

    public async Task UpdateProduct(int id, CreateProductDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            ProductCategoryId = productDTO.ProductCategoryId,
            Price = productDTO.Price,
            Stock = productDTO.Stock
        };
        await _productRepository.UpdateProduct(product);
    }

    public async Task DeleteProduct(int id)
    {
        await _productRepository.DeleteProduct(id);
    }
}