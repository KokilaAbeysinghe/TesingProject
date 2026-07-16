using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Application.Services;

public class ReportService : IReportService
{
    private readonly ISaleRepository _saleRepository;

    public ReportService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SalesSummaryDTO> GetSalesSummary(DateTime startDate, DateTime endDate)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(startDate.Date, ToExclusiveEndDate(endDate));

        return new SalesSummaryDTO
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalSalesCount = sales.Count,
            TotalItemsSold = sales.Sum(s => s.SaleItems.Sum(si => si.Quantity)),
            TotalRevenue = sales.Sum(s => s.TotalAmount)
        };
    }

    public async Task<List<TopProductDTO>> GetTopProducts(DateTime startDate, DateTime endDate, int count)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(startDate.Date, ToExclusiveEndDate(endDate));

        var topProducts = sales
            .SelectMany(s => s.SaleItems)
            .GroupBy(si => new { si.ProductId, si.Product.Name })
            .Select(group => new TopProductDTO
            {
                ProductId = group.Key.ProductId,
                ProductName = group.Key.Name,
                QuantitySold = group.Sum(si => si.Quantity),
                Revenue = group.Sum(si => si.Quantity * si.UnitPrice)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(count)
            .ToList();

        return topProducts;
    }

    private static DateTime ToExclusiveEndDate(DateTime endDate) => endDate.Date.AddDays(1);
}
