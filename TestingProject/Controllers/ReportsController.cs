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


    [HttpGet("monthly-sales")]
    public async Task<IActionResult> GetMonthlySalesSummary(DateTime startDate, DateTime endDate)
    {
        var monthlySalesSummary = await _reportService.GetMonthlySalesSummary(startDate, endDate);
        return Ok(monthlySalesSummary);
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

    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopCustomers(DateTime startDate, DateTime endDate)
    {
        var topCustomers = await _reportService.GetTopCustomers(startDate, endDate);
        return Ok(topCustomers);
    }

    [HttpGet("daily-sales")]
    public async Task<IActionResult> GetDailySalesSummary(DateTime startDate, DateTime endDate)
    {
        var dailySalesSummary = await _reportService.GetDailySalesSummary(startDate, endDate);
        return Ok(dailySalesSummary);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStockProducts()
    {
        var lowStockProducts = await _reportService.GetLowStockProducts();
        return Ok(lowStockProducts);
    }




    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportSalesReportToExcel(DateTime startDate, DateTime endDate, string reportType = "summary")
    {
        if (startDate > endDate)
            return BadRequest("Start date must be before or equal to end date.");

        var fileContent = await _reportService.ExportSalesReportToExcel(startDate, endDate, reportType);
        var reportTypeFileNamePart = reportType switch
        {
            "topProducts" => "top-products",
            "paymentMethods" => "payment-methods",
            "dailySales" => "daily-sales",
            "lowStock" => "low-stock",
            _ => "summary"
        };
        var fileName = $"{reportTypeFileNamePart}-report_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}.xlsx";

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
