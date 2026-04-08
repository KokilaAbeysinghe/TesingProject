using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

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

    [HttpGet]
    public async Task<IActionResult> GetAllSales()
    {
        var sales = await _saleService.GetAllSales();
        return Ok(sales);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sale = await _saleService.GetSaleById(id);
        if (sale == null)
            return NotFound("Sale not found!");
        return Ok(sale);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale(CreateSaleDTO saleDTO)
    {
        await _saleService.CreateSale(saleDTO);
        return Ok("Sale created successfully!");
    }

    [HttpGet("{id}/total")]
    public async Task<IActionResult> CalculateTotal(int id)
    {
        var total = await _saleService.CalculateTotal(id);
        return Ok(new { SaleId = id, Total = total });
    }
}