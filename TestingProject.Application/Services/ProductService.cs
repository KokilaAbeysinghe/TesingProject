using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

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
            Category = p.Category,
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
            Category = product.Category,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task AddProduct(CreateProductDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Category = productDTO.Category,
            Price = productDTO.Price,
            Stock = productDTO.Stock
        };
        await _productRepository.AddProduct(product);
    }

    public async Task UpdateProduct(int id, CreateProductDTO productDTO)
    {
        var product = new Product
        {
            Id = id,
            Name = productDTO.Name,
            Category = productDTO.Category,
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