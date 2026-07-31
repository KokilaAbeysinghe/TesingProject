using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetCustomersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var customers = await _customerService.GetCustomersPaged(pageNumber, pageSize);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerById(id);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer(CreateCustomerDTO customerDTO)
    {
        await _customerService.AddCustomer(customerDTO);
        return Ok("Customer added successfully!");



    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDTO customerDTO)
    {
        await _customerService.UpdateCustomer(id, customerDTO);
        return Ok("Customer updated successfully!");



    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customerService.DeleteCustomer(id);
        return Ok("Customer deleted successfully!");



    }
}