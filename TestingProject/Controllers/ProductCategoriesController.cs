using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;


namespace TestingProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _categoryService;

    public ProductCategoriesController(IProductCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/productcategories
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategories();
        return Ok(categories);
    }

    // GET: api/productcategories/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryById(id);
        return Ok(category);
    }

    // POST: api/productcategories
    [HttpPost]
    public async Task<IActionResult> AddCategory(CreateProductCategoryDTO categoryDTO)
    {
        await _categoryService.AddCategory(categoryDTO);
        return Ok("Category added successfully!");
    }

    // PUT: api/productcategories/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CreateProductCategoryDTO categoryDTO)
    {
        await _categoryService.UpdateCategory(id, categoryDTO);
        return Ok("Category updated successfully!");
    }

    // DELETE: api/productcategories/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategory(id);
        return Ok("Category deleted successfully!");
    }
}
