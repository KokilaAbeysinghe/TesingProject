using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize(Roles = "Admin,Manager")]
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

    [HttpGet("payment-methods")]
    public async Task<IActionResult> GetPaymentMethodSummary(DateTime startDate, DateTime endDate)
    {
        var paymentMethodSummary = await _reportService.GetPaymentMethodSummary(startDate, endDate);
        return Ok(paymentMethodSummary);
    }

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportSalesReportToExcel(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be before or equal to end date.");

        var fileContent = await _reportService.ExportSalesReportToExcel(startDate, endDate);
        var fileName = $"sales-report_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}.xlsx";

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
