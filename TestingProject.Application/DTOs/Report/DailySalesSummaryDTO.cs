namespace TestingProject.Application.DTOs;

public record DailySalesSummaryDTO
{
    public DateTime Date { get; init; }
    public int SalesCount { get; init; }
    public decimal TotalRevenue { get; init; }
}
