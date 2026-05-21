
using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public class CreateSaleItemDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid product is required.")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 9999, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}