using TestingProject.Domain.Enums;

namespace TestingProject.Application.DTOs;

public class SaleDTO
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public SaleStatus Status { get; set; }
    public List<SaleItemDTO> SaleItems { get; set; } = new();
}