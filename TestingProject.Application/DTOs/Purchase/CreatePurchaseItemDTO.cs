using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public record CreatePurchaseItemDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid product is required.")]
    public int ProductId { get; init; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; init; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than 0.")]
    public decimal UnitCost { get; init; }
}
