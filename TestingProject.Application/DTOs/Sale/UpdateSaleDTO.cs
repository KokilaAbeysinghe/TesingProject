using System.ComponentModel.DataAnnotations;
using TestingProject.Domain.Enums;

namespace TestingProject.Application.DTOs;

public record UpdateSaleDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid customer is required.")]
    public int CustomerId { get; init; }
    [Range(0, 100, ErrorMessage = "Discount percentage must be a whole number between 0 and 100.")]
    public int DiscountPercentage { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
}
