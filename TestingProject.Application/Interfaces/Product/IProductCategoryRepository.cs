
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface IProductCategoryRepository
{
    Task<List<ProductCategory>> GetAllCategories();
    Task<ProductCategory> GetCategoryById(int id);
    Task AddCategory(ProductCategory category);
    Task UpdateCategory(ProductCategory category);
    Task DeleteCategory(int id);
}