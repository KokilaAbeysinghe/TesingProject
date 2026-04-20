using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _categoryRepository;

    public ProductCategoryService(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ProductCategoryDTO>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();
        return categories.Select(c => new ProductCategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();
    }

    public async Task<ProductCategoryDTO> GetCategoryById(int id)
    {
        var category = await _categoryRepository.GetCategoryById(id);

        if (category == null)
            throw new KeyNotFoundException(
                $"Category with ID {id} not found!");

        return new ProductCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task AddCategory(CreateProductCategoryDTO categoryDTO)
    {
        var category = new ProductCategory
        {
            Name = categoryDTO.Name,
            Description = categoryDTO.Description
        };
        await _categoryRepository.AddCategory(category);
    }

    public async Task UpdateCategory(int id, CreateProductCategoryDTO categoryDTO)
    {
        var category = new ProductCategory
        {
            Id = id,
            Name = categoryDTO.Name,
            Description = categoryDTO.Description
        };
        await _categoryRepository.UpdateCategory(category);
    }

    public async Task DeleteCategory(int id)
    {
        await _categoryRepository.DeleteCategory(id);
    }
}