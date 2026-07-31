namespace TestingProject.Application.DTOs;

public record MonthlySalesSummaryDTO
{
    public string Month { get; init; } = string.Empty;
    public int TransactionCount { get; init; }
    public decimal TotalRevenue { get; init; }
    public decimal AverageSaleValue { get; init; }
}
