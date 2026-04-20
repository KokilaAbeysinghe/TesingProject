
using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllProducts();
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(CreateProductDTO productDTO);
    Task UpdateProduct(int id, CreateProductDTO productDTO);
    Task DeleteProduct(int id);
}