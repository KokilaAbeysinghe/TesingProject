using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;

namespace TestingProject.Infrastructure.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _context;

    public PurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Purchase>> GetAllPurchases()
    {
        return await _context.Purchases
            .Include(purchase => purchase.Supplier)
            .Include(purchase => purchase.PurchaseItems)
            .ThenInclude(item => item.Product)
            .OrderByDescending(purchase => purchase.PurchaseDate)
            .ToListAsync();
    }

    public async Task<(List<Purchase> Items, int TotalCount)> GetPurchasesPaged(int pageNumber, int pageSize)
    {
        var query = _context.Purchases
            .Include(purchase => purchase.Supplier)
            .Include(purchase => purchase.PurchaseItems)
            .ThenInclude(item => item.Product)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(purchase => purchase.PurchaseDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Purchase?> GetPurchaseById(int id)
    {
        return await _context.Purchases
            .Include(purchase => purchase.Supplier)
            .Include(purchase => purchase.PurchaseItems)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(purchase => purchase.Id == id);
    }

    public async Task CreatePurchase(Purchase purchase)
    {
        await _context.Purchases.AddAsync(purchase);
        await _context.SaveChangesAsync();
    }
}
