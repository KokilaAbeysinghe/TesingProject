
using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IProductCategoryService
{
    Task<List<ProductCategoryDTO>> GetAllCategories();
    Task<PagedResultDTO<ProductCategoryDTO>> GetCategoriesPaged(int pageNumber, int pageSize);
    Task<ProductCategoryDTO> GetCategoryById(int id);
    Task AddCategory(CreateProductCategoryDTO categoryDTO);
    Task UpdateCategory(int id, CreateProductCategoryDTO categoryDTO);
    Task DeleteCategory(int id);
}