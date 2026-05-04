using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;

namespace TestingProject.Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly AppDbContext _context;

    public ProductCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductCategory>> GetAllCategories()
    {
        return await _context.ProductCategories
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task<ProductCategory> GetCategoryById(int id)
    {
        return await _context.ProductCategories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCategory(ProductCategory category)
    {
        await _context.ProductCategories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategory(ProductCategory category)
    {
        var existing = await _context.ProductCategories
            .FindAsync(category.Id);

        if (existing == null)
            throw new KeyNotFoundException(
                $"Category with ID {category.Id} not found!");

        existing.Name = category.Name;
        existing.Description = category.Description;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _context.ProductCategories
            .FindAsync(id);

        if (category == null)
            throw new KeyNotFoundException(
                $"Category with ID {id} not found!");

        _context.ProductCategories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
