using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers()
    {
        var suppliers = await _supplierService.GetAllSuppliers();

        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        var supplier = await _supplierService.GetSupplierById(id);

        return Ok(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AddSupplier(CreateSupplierDTO supplierDTO)
    {
        await _supplierService.AddSupplier(supplierDTO);

        return Ok(new { message = "Supplier added successfully!" });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateSupplier(int id, CreateSupplierDTO supplierDTO)
    {
        await _supplierService.UpdateSupplier(id, supplierDTO);

        return Ok(new { message = "Supplier updated successfully!" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        await _supplierService.DeleteSupplier(id);

        return Ok(new { message = "Supplier deleted successfully!" });
    }
}
