using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDTO>> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllCustomers();
        return customers.Select(c => new CustomerDTO
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
           
        }).ToList();
    }

    public async Task<CustomerDTO> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);

        if (customer == null)
            throw new KeyNotFoundException(
                $"Customer with ID {id} not found!");

        return new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Phone = customer.Phone,
            
        };
    }

    public async Task AddCustomer(CreateCustomerDTO customerDTO)
    {
        var customer = new Customer
        {
            Name = customerDTO.Name,
            Phone = customerDTO.Phone,
           
        };
        await _customerRepository.AddCustomer(customer);
    }

    public async Task UpdateCustomer(int id, CreateCustomerDTO customerDTO)
    {
        var customer = new Customer
        {
            Id = id,
            Name = customerDTO.Name,
            Phone = customerDTO.Phone,
           
        };
        await _customerRepository.UpdateCustomer(customer);
    }

    public async Task DeleteCustomer(int id)
    {
        await _customerRepository.DeleteCustomer(id);
    }
}
