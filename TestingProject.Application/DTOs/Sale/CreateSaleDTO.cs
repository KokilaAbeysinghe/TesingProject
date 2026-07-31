using System.ComponentModel.DataAnnotations;
using TestingProject.Domain.Enums;

namespace TestingProject.Application.DTOs;

public class CreateSaleDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid customer is required.")]
    public int CustomerId { get; set; }
    [Required]
    [MinLength(1, ErrorMessage = "At least one sale item is required.")]
    public List<CreateSaleItemDTO> SaleItems { get; set; } = new();
    [Range(0, 100, ErrorMessage = "Discount percentage must be a whole number between 0 and 100.")]
    public int DiscountPercentage { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}