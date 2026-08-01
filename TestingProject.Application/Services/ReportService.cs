using ClosedXML.Excel;
using TestingProject.Application.DTOs;
using TestingProject.Application.DTOs.Report;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Enums;

namespace TestingProject.Application.Services;

public class ReportService : IReportService
{
    private const int LowStockReorderLevel = 10;

    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public ReportService(ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<List<MonthlySalesSummaryDTO>> GetMonthlySalesSummary()
    {
        var sales = await _saleRepository.GetAllSalesForSummary();

        var monthlySalesSummary = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .GroupBy(sale =>
            {
                var saleDate = ToLocalDate(sale.SaleDate);

                return new DateTime(saleDate.Year, saleDate.Month, 1);
            })
            .OrderBy(group => group.Key)
            .Select(group => new MonthlySalesSummaryDTO
            {
                Month = group.Key.ToString("MMM yyyy"),
                TransactionCount = group.Count(),
                TotalRevenue = group.Sum(sale => sale.TotalAmount),
                AverageSaleValue = group.Sum(sale => sale.TotalAmount) / group.Count()
            })
            .ToList();

        return monthlySalesSummary;
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
            .GroupBy(si => new
            {
                si.ProductId,
                ProductName = si.Product.Name,
                CategoryName = si.Product.ProductCategory is not null ? si.Product.ProductCategory.Name : "Uncategorized"
            })
            .Select(group => new TopProductDTO
            {
                ProductId = group.Key.ProductId,
                ProductName = group.Key.ProductName,
                CategoryName = group.Key.CategoryName,
                QuantitySold = group.Sum(si => si.Quantity),
                Revenue = group.Sum(si => si.Quantity * si.UnitPrice)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(count)
            .ToList();
        //.OrderByDescending(p => p.Revenue)

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

    public async Task<byte[]> ExportSalesReportToExcel(string reportType, DateTime? startDate = null, DateTime? endDate = null)
    {
        using var workbook = new XLWorkbook();

        switch (reportType)
        {
            case "topProducts":
                await AddTopProductsSheet(workbook, startDate!.Value, endDate!.Value);
                break;

            case "paymentMethods":
                await AddPaymentMethodsSheet(workbook, startDate!.Value, endDate!.Value);
                break;

            case "topCustomers":
                await AddTopCustomersSheet(workbook, startDate!.Value, endDate!.Value);
                break;

            case "dailySales":
                await AddDailySalesSheet(workbook, startDate!.Value, endDate!.Value);
                break;

            case "lowStock":
                await AddLowStockSheet(workbook);
                break;

            default:
                await AddSummarySheet(workbook);
                break;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public async Task<List<TopCustomerDTO>> GetTopCustomers(DateTime startDate, DateTime endDate)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(
            ToUtcStartDate(startDate),
            ToUtcExclusiveEndDate(endDate));

        var topCustomers = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .GroupBy(sale => sale.Customer)
            .Select(group => new TopCustomerDTO
            {
                CustomerId = group.Key.Id,
                CustomerName = $"{group.Key.Name} {group.Key.LastName}".Trim(),
                QuantityBuy = group.Count(),
                CustomerAmount = group.Sum(sale => sale.TotalAmount)
            })
            .OrderByDescending(summary => summary.CustomerAmount)
            .ToList();

        return topCustomers;
    }

    public async Task<List<DailySalesSummaryDTO>> GetDailySalesSummary(DateTime startDate, DateTime endDate)
    {
        var sales = await _saleRepository.GetSalesBetweenDates(
            ToUtcStartDate(startDate),
            ToUtcExclusiveEndDate(endDate));

        var dailySalesSummary = sales
            .Where(sale => sale.Status != SaleStatus.Voided)
            .GroupBy(sale => ToLocalDate(sale.SaleDate))
            .Select(group => new DailySalesSummaryDTO
            {
                Date = group.Key,
                SalesCount = group.Count(),
                TotalRevenue = group.Sum(sale => sale.TotalAmount)
            })
            .OrderBy(summary => summary.Date)
            .ToList();

        return dailySalesSummary;
    }

    public async Task<List<LowStockProductDTO>> GetLowStockProducts()
    {
        var products = await _productRepository.GetAllProducts();

        var lowStockProducts = products
            .Where(product => product.Stock <= LowStockReorderLevel)
            .OrderBy(product => product.Stock)
            .Select(product => new LowStockProductDTO
            {
                ProductName = product.Name,
                CurrentStock = product.Stock,
                ReorderLevel = LowStockReorderLevel,
                Status = product.Stock == 0 ? "Out of Stock" : "Low Stock"
            })
            .ToList();

        return lowStockProducts;
    }


    private async Task AddSummarySheet(XLWorkbook workbook)
    {
        var monthlySalesSummary = await GetMonthlySalesSummary();

        var summarySheet = workbook.Worksheets.Add("Summary");
        summarySheet.Cell(1, 1).Value = "Month";
        summarySheet.Cell(1, 2).Value = "Transactions";
        summarySheet.Cell(1, 3).Value = "Total Revenue (LKR)";
        summarySheet.Cell(1, 4).Value = "Average Sale Value (LKR)";
        summarySheet.Range(1, 1, 1, 4).Style.Font.Bold = true;

        var row = 2;
        foreach (var monthSummary in monthlySalesSummary)
        {
            summarySheet.Cell(row, 1).Value = monthSummary.Month;
            summarySheet.Cell(row, 2).Value = monthSummary.TransactionCount;
            summarySheet.Cell(row, 3).Value = monthSummary.TotalRevenue;
            summarySheet.Cell(row, 4).Value = monthSummary.AverageSaleValue;
            row++;
        }

        summarySheet.Columns().AdjustToContents();
    }

    private async Task AddTopProductsSheet(XLWorkbook workbook, DateTime startDate, DateTime endDate)
    {
        var topProducts = await GetTopProducts(startDate, endDate, 10);

        var topProductsSheet = workbook.Worksheets.Add("Top Products");
        topProductsSheet.Cell(1, 1).Value = "Product";
        topProductsSheet.Cell(1, 2).Value = "Category";
        topProductsSheet.Cell(1, 3).Value = "Qty Sold";
        topProductsSheet.Cell(1, 4).Value = "Revenue (LKR)";
        topProductsSheet.Range(1, 1, 1, 4).Style.Font.Bold = true;

        var row = 2;
        foreach (var product in topProducts)
        {
            topProductsSheet.Cell(row, 1).Value = product.ProductName;
            topProductsSheet.Cell(row, 2).Value = product.CategoryName;
            topProductsSheet.Cell(row, 3).Value = product.QuantitySold;
            topProductsSheet.Cell(row, 4).Value = product.Revenue;
            row++;
        }

        topProductsSheet.Columns().AdjustToContents();
    }

    private async Task AddPaymentMethodsSheet(XLWorkbook workbook, DateTime startDate, DateTime endDate)
    {
        var paymentMethodSummary = await GetPaymentMethodSummary(startDate, endDate);

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
    }

    private async Task AddTopCustomersSheet(XLWorkbook workbook, DateTime startDate, DateTime endDate)
    {
        var topCustomers = await GetTopCustomers(startDate, endDate);

        var sheet = workbook.Worksheets.Add("Top Customers");
        sheet.Cell(1, 1).Value = "Customer Name";
        sheet.Cell(1, 2).Value = "Quantity Buy";
        sheet.Cell(1, 3).Value = "Amount (LKR)";
        sheet.Range(1, 1, 1, 3).Style.Font.Bold = true;

        var row = 2;
        foreach (var customer in topCustomers)
        {
            sheet.Cell(row, 1).Value = customer.CustomerName;
            sheet.Cell(row, 2).Value = customer.QuantityBuy;
            sheet.Cell(row, 3).Value = customer.CustomerAmount;
            row++;
        }

        sheet.Columns().AdjustToContents();
    }

    private async Task AddDailySalesSheet(XLWorkbook workbook, DateTime startDate, DateTime endDate)
    {
        var dailySalesSummary = await GetDailySalesSummary(startDate, endDate);

        var sheet = workbook.Worksheets.Add("Daily Sales Summary");
        sheet.Cell(1, 1).Value = "Date";
        sheet.Cell(1, 2).Value = "No of Sales";
        sheet.Cell(1, 3).Value = "Total Revenue (LKR)";
        sheet.Range(1, 1, 1, 3).Style.Font.Bold = true;

        var row = 2;
        foreach (var dailySummary in dailySalesSummary)
        {
            sheet.Cell(row, 1).Value = dailySummary.Date.ToString("yyyy-MM-dd");
            sheet.Cell(row, 2).Value = dailySummary.SalesCount;
            sheet.Cell(row, 3).Value = dailySummary.TotalRevenue;
            row++;
        }

        sheet.Columns().AdjustToContents();
    }

    private async Task AddLowStockSheet(XLWorkbook workbook)
    {
        var lowStockProducts = await GetLowStockProducts();

        var sheet = workbook.Worksheets.Add("Low Stock");
        sheet.Cell(1, 1).Value = "Product";
        sheet.Cell(1, 2).Value = "Current Stock";
        sheet.Cell(1, 3).Value = "Reorder Level";
        sheet.Cell(1, 4).Value = "Status";
        sheet.Range(1, 1, 1, 4).Style.Font.Bold = true;

        var row = 2;
        foreach (var product in lowStockProducts)
        {
            sheet.Cell(row, 1).Value = product.ProductName;
            sheet.Cell(row, 2).Value = product.CurrentStock;
            sheet.Cell(row, 3).Value = product.ReorderLevel;
            sheet.Cell(row, 4).Value = product.Status;
            row++;
        }

        sheet.Columns().AdjustToContents();
    }

    private static DateTime ToLocalDate(DateTime dateTime)
    {
        var localDateTime = dateTime.Kind == DateTimeKind.Utc
            ? dateTime.ToLocalTime()
            : dateTime;

        return localDateTime.Date;
    }

    private static DateTime ToUtcStartDate(DateTime startDate) =>
        DateTime.SpecifyKind(startDate.Date, DateTimeKind.Local).ToUniversalTime();

    private static DateTime ToUtcExclusiveEndDate(DateTime endDate) =>
        DateTime.SpecifyKind(endDate.Date.AddDays(1), DateTimeKind.Local).ToUniversalTime();
}
