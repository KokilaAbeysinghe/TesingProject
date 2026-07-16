using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IReportService
{
    Task<SalesSummaryDTO> GetSalesSummary(DateTime startDate, DateTime endDate);
    Task<List<TopProductDTO>> GetTopProducts(DateTime startDate, DateTime endDate, int count);
}
