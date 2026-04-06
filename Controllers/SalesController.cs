using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    // GET: api/sales
    [HttpGet]
    public async Task<IActionResult> GetAllSales()
    {
        var sales = await _saleService.GetAllSales();
        return Ok(sales);
    }

    // GET: api/sales/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sale = await _saleService.GetSaleById(id);
        if (sale == null)
            return NotFound("Sale not found!");
        return Ok(sale);
    }

    // POST: api/sales
    [HttpPost]
    public async Task<IActionResult> CreateSale(Sale sale)
    {
        await _saleService.CreateSale(sale);
        return Ok("Sale created successfully!");
    }

    // GET: api/sales/1/total
    [HttpGet("{id}/total")]
    public async Task<IActionResult> CalculateTotal(int id)
    {
        var total = await _saleService.CalculateTotal(id);
        return Ok(new { SaleId = id, Total = total });
    }
}