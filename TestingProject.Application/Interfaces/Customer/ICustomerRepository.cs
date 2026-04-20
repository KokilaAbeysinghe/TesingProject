using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllCustomers();
    Task<Customer> GetCustomerById(int id);
    Task AddCustomer(Customer customer);
    Task UpdateCustomer(Customer customer);
    Task DeleteCustomer(int id);
}