using Microsoft.EntityFrameworkCore;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;
using TestingProject.Application.Interfaces;

namespace TestingProject.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetAllSales()
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .ToListAsync();
    }

    public async Task<(List<Sale> Items, int TotalCount)> GetSalesPaged(int pageNumber, int pageSize)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(s => s.SaleDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Sale?> GetSaleById(int id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task CreateSale(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSale(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sale>> GetSalesBetweenDates(DateTime startDate, DateTime endDate)
    {
        var utcStartDate = startDate.Kind == DateTimeKind.Utc
            ? startDate
            : DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEndDate = endDate.Kind == DateTimeKind.Utc
            ? endDate
            : DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .ThenInclude(p => p.ProductCategory)
            .Where(s => s.SaleDate >= utcStartDate && s.SaleDate < utcEndDate)
            .ToListAsync();
    }
}