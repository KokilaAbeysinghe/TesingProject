using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public class CreateProductDTO
{
    [Required(ErrorMessage = "Product name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required!")]
    public string Category { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999, ErrorMessage = "Price must be greater than 0!")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, 99999, ErrorMessage = "Stock cannot be negative!")]
    public int Stock { get; set; }
}