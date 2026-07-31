using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllProducts();
    Task<PagedResultDTO<ProductDTO>> GetProductsPaged(int pageNumber, int pageSize, string? search, int? maxStock);
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(CreateProductDTO productDTO);
    Task UpdateProduct(int id, CreateProductDTO productDTO);
    Task AdjustStock(int id, AdjustStockDTO adjustStockDTO);
    Task DeleteProduct(int id);
    Task<int> GetProductsCount();
}