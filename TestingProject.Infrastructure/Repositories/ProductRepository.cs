using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;

namespace TestingProject.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.Include(p => p.ProductCategory).ToListAsync();
    }

    public async Task<(List<Product> Items, int TotalCount)> GetProductsPaged(int pageNumber, int pageSize, string? search, int? maxStock)
    {
        var query = _context.Products.Include(p => p.ProductCategory).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, pattern)
                || (p.ProductCategory != null && EF.Functions.ILike(p.ProductCategory.Name, pattern)));
        }

        if (maxStock.HasValue)
        {
            query = query.Where(p => p.Stock <= maxStock.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
    public async Task UpdateStock(int productId, int newStock)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            throw new KeyNotFoundException($"Product with ID {productId} not found.");

        product.Stock = newStock;
        await _context.SaveChangesAsync();
    }
}