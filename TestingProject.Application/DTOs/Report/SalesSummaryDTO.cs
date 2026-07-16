namespace TestingProject.Application.DTOs;

public record SalesSummaryDTO
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int TotalSalesCount { get; init; }
    public int TotalItemsSold { get; init; }
    public decimal TotalRevenue { get; init; }
}
