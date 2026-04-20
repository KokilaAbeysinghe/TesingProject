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

    public async Task<Sale> GetSaleById(int id)
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
}