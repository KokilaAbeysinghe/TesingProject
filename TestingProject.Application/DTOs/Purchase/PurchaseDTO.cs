namespace TestingProject.Application.DTOs;

public record PurchaseDTO
{
    public int Id { get; init; }
    public DateTime PurchaseDate { get; init; }
    public int SupplierId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public List<PurchaseItemDTO> PurchaseItems { get; init; } = new();
}
