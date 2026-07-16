using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public record AdjustStockDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; init; }

    [Required]
    public string AdjustmentType { get; init; } = "Add";
}
