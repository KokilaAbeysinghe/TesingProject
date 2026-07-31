using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchasesController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPurchases()
    {
        var purchases = await _purchaseService.GetAllPurchases();

        return Ok(purchases);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPurchasesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var purchases = await _purchaseService.GetPurchasesPaged(pageNumber, pageSize);

        return Ok(purchases);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseById(int id)
    {
        var purchase = await _purchaseService.GetPurchaseById(id);

        return Ok(purchase);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreatePurchase(CreatePurchaseDTO purchaseDTO)
    {
        await _purchaseService.CreatePurchase(purchaseDTO);

        return Ok(new { message = "Purchase recorded successfully!" });
    }
}
