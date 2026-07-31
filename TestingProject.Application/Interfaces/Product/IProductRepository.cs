using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();
    Task<(List<Product> Items, int TotalCount)> GetProductsPaged(int pageNumber, int pageSize, string? search, int? maxStock);
    Task<Product?> GetProductById(int id);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int id);
    Task UpdateStock(int productId, int newStock);
}