namespace TestingProject.Application.DTOs;

public record TopProductDTO
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public int QuantitySold { get; init; }
    public decimal Revenue { get; init; }
}
