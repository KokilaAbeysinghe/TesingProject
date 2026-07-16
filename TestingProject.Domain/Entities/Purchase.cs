namespace TestingProject.Domain.Entities;

public class Purchase
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public List<PurchaseItem> PurchaseItems { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
