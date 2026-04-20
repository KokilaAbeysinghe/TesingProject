namespace TestingProject.Application.DTOs;

public class SaleDTO
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<SaleItemDTO> SaleItems { get; set; } = new();
}