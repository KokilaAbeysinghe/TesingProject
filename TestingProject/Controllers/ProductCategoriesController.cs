using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _categoryService;

    public ProductCategoriesController(IProductCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetCategoriesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var categories = await _categoryService.GetCategoriesPaged(pageNumber, pageSize);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryById(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CreateProductCategoryDTO categoryDTO)
    {
        await _categoryService.AddCategory(categoryDTO);
        return Ok(new { message = "Category added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CreateProductCategoryDTO categoryDTO)
    {
        await _categoryService.UpdateCategory(id, categoryDTO);
        return Ok(new { message = "Category updated successfully!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategory(id);
        return Ok(new { message = "Category deleted successfully!" });
    }
}
