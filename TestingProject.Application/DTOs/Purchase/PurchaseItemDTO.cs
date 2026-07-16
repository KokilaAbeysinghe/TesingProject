namespace TestingProject.Application.DTOs;

public record PurchaseItemDTO
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitCost { get; init; }
}
