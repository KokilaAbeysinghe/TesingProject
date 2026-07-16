using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSalesSummary(DateTime startDate, DateTime endDate)
    {
        var summary = await _reportService.GetSalesSummary(startDate, endDate);
        return Ok(summary);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> GetTopProducts(DateTime startDate, DateTime endDate, int count = 5)
    {
        var topProducts = await _reportService.GetTopProducts(startDate, endDate, count);
        return Ok(topProducts);
    }
}
