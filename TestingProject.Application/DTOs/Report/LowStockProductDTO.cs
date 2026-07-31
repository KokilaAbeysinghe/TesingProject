namespace TestingProject.Application.DTOs;

public record LowStockProductDTO
{
    public string ProductName { get; init; } = string.Empty;
    public int CurrentStock { get; init; }
    public int ReorderLevel { get; init; }
    public string Status { get; init; } = string.Empty;
}
