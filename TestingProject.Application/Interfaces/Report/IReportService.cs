using TestingProject.Application.DTOs;
using TestingProject.Application.DTOs.Report;

namespace TestingProject.Application.Interfaces;

public interface IReportService
{
    Task<List<MonthlySalesSummaryDTO>> GetMonthlySalesSummary();
    Task<List<TopProductDTO>> GetTopProducts(DateTime startDate, DateTime endDate, int count);
    Task<List<PaymentMethodSummaryDTO>> GetPaymentMethodSummary(DateTime startDate, DateTime endDate);
    Task<byte[]> ExportSalesReportToExcel(string reportType, DateTime? startDate = null, DateTime? endDate = null);
    Task<List<TopCustomerDTO>> GetTopCustomers(DateTime startDate, DateTime endDate);
    Task<List<DailySalesSummaryDTO>> GetDailySalesSummary(DateTime startDate, DateTime endDate);
    Task<List<LowStockProductDTO>> GetLowStockProducts();
}
