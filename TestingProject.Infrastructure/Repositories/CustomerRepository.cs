using Microsoft.EntityFrameworkCore;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Infrastructure.Data;

namespace TestingProject.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task AddCustomer(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomer(Customer customer)
    {
        var existing = await _context.Customers.FindAsync(customer.Id);

        if (existing == null)
            throw new KeyNotFoundException(
                $"Customer with ID {customer.Id} not found!");

        existing.Name = customer.Name;
        existing.Phone = customer.Phone;
       
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            throw new KeyNotFoundException(
                $"Customer with ID {id} not found!");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}