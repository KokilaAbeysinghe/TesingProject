using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public record CreatePurchaseDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid supplier is required.")]
    public int SupplierId { get; init; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one purchase item is required.")]
    public List<CreatePurchaseItemDTO> PurchaseItems { get; init; } = new();
}
