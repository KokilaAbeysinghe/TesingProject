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
    public PaymentMethod PaymentMethod { get; set; }
}