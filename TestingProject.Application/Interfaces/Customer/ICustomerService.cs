using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDTO>> GetAllCustomers();
    Task<PagedResultDTO<CustomerDTO>> GetCustomersPaged(int pageNumber, int pageSize);
    Task<CustomerDTO> GetCustomerById(int id);
    Task AddCustomer(CreateCustomerDTO customerDTO);
    Task UpdateCustomer(int id, CreateCustomerDTO customerDTO);
    Task DeleteCustomer(int id);
}