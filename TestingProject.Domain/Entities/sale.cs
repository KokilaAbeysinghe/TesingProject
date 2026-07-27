
using TestingProject.Domain.Enums;

namespace TestingProject.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public List<SaleItem> SaleItems { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Completed;
}