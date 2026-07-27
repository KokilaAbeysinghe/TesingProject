using ClosedXML.Excel;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Enums;

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
        var sales = await _saleRepository.GetSalesBetweenDates(
            ToUtcStartDate(startDate),
            ToUtcExclusiveEndDate(endDate));

        var completedSales = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .ToList();

        return new SalesSummaryDTO
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalSalesCount = completedSales.Count,
            TotalItemsSold = completedSales.Sum(s => s.SaleItems.Sum(si => si.Quantity)),
            TotalRevenue = completedSales.Sum(s => s.TotalAmount)
        };
    }

    public async Task<List<TopProductDTO>> GetTopProducts(DateTime startDate, DateTime endDate, int count)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(
            ToUtcStartDate(startDate),
            ToUtcExclusiveEndDate(endDate));

        var topProducts = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .SelectMany(s => s.SaleItems)
            .Where(si => si.Product is not null)
            .GroupBy(si => new { si.ProductId, ProductName = si.Product.Name })
            .Select(group => new TopProductDTO
            {
                ProductId = group.Key.ProductId,
                ProductName = group.Key.ProductName,
                QuantitySold = group.Sum(si => si.Quantity),
                Revenue = group.Sum(si => si.Quantity * si.UnitPrice)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(count)
            .ToList();

        return topProducts;
    }

    public async Task<List<PaymentMethodSummaryDTO>> GetPaymentMethodSummary(DateTime startDate, DateTime endDate)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(
            ToUtcStartDate(startDate),
            ToUtcExclusiveEndDate(endDate));

        var paymentMethodSummary = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .GroupBy(sale => sale.PaymentMethod)
            .Select(group => new PaymentMethodSummaryDTO
            {
                PaymentMethod = group.Key,
                SalesCount = group.Count(),
                TotalAmount = group.Sum(sale => sale.TotalAmount)
            })
            .OrderByDescending(summary => summary.TotalAmount)
            .ToList();

        return paymentMethodSummary;
    }

    public async Task<byte[]> ExportSalesReportToExcel(DateTime startDate, DateTime endDate)
    {
        var summary = await GetSalesSummary(startDate, endDate);
        var topProducts = await GetTopProducts(startDate, endDate, 10);
        var paymentMethodSummary = await GetPaymentMethodSummary(startDate, endDate);

        using var workbook = new XLWorkbook();

        var summarySheet = workbook.Worksheets.Add("Summary");
        summarySheet.Cell(1, 1).Value = "Sales Report";
        summarySheet.Cell(2, 1).Value = "Start Date";
        summarySheet.Cell(2, 2).Value = summary.StartDate.ToString("yyyy-MM-dd");
        summarySheet.Cell(3, 1).Value = "End Date";
        summarySheet.Cell(3, 2).Value = summary.EndDate.ToString("yyyy-MM-dd");
        summarySheet.Cell(4, 1).Value = "Total Revenue (LKR)";
        summarySheet.Cell(4, 2).Value = summary.TotalRevenue;
        summarySheet.Cell(5, 1).Value = "Total Sales Count";
        summarySheet.Cell(5, 2).Value = summary.TotalSalesCount;
        summarySheet.Cell(6, 1).Value = "Total Items Sold";
        summarySheet.Cell(6, 2).Value = summary.TotalItemsSold;
        summarySheet.Column(1).Width = 22;
        summarySheet.Column(2).Width = 18;


        var topProductsSheet = workbook.Worksheets.Add("Top Products");
        topProductsSheet.Cell(1, 1).Value = "Product";
        topProductsSheet.Cell(1, 2).Value = "Quantity Sold";
        topProductsSheet.Cell(1, 3).Value = "Revenue (LKR)";
        topProductsSheet.Range(1, 1, 1, 3).Style.Font.Bold = true;

        var row = 2;
        foreach (var product in topProducts)
        {
            topProductsSheet.Cell(row, 1).Value = product.ProductName;
            topProductsSheet.Cell(row, 2).Value = product.QuantitySold;
            topProductsSheet.Cell(row, 3).Value = product.Revenue;
            row++;
        }

        topProductsSheet.Columns().AdjustToContents();

        var paymentMethodSheet = workbook.Worksheets.Add("Payment Methods");
        paymentMethodSheet.Cell(1, 1).Value = "Payment Method";
        paymentMethodSheet.Cell(1, 2).Value = "Sales Count";
        paymentMethodSheet.Cell(1, 3).Value = "Total Amount (LKR)";
        paymentMethodSheet.Range(1, 1, 1, 3).Style.Font.Bold = true;

        var paymentMethodRow = 2;
        foreach (var paymentMethod in paymentMethodSummary)
        {
            paymentMethodSheet.Cell(paymentMethodRow, 1).Value = paymentMethod.PaymentMethod.ToString();
            paymentMethodSheet.Cell(paymentMethodRow, 2).Value = paymentMethod.SalesCount;
            paymentMethodSheet.Cell(paymentMethodRow, 3).Value = paymentMethod.TotalAmount;
            paymentMethodRow++;
        }

        paymentMethodSheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private static DateTime ToUtcStartDate(DateTime startDate) =>
        DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);

    private static DateTime ToUtcExclusiveEndDate(DateTime endDate) =>
        DateTime.SpecifyKind(endDate.Date.AddDays(1), DateTimeKind.Utc);
}
