using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;

namespace TestingProject.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllSuppliers()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task<Supplier?> GetSupplierById(int id)
    {
        return await _context.Suppliers.FindAsync(id);
    }

    public async Task AddSupplier(Supplier supplier)
    {
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplier(Supplier supplier)
    {
        var existing = await _context.Suppliers.FindAsync(supplier.Id);

        if (existing is null)
            throw new KeyNotFoundException($"Supplier with ID {supplier.Id} not found!");

        existing.Name = supplier.Name;
        existing.Phone = supplier.Phone;
        existing.Email = supplier.Email;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier is null)
            throw new KeyNotFoundException($"Supplier with ID {id} not found!");

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}
