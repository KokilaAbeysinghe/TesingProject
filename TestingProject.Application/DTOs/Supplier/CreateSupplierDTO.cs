using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public record CreateSupplierDTO
{
    [Required(ErrorMessage = "Supplier name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required!")]
    [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters!")]
    [RegularExpression(@"^(?:\+94|0)[0-9]{9}$", ErrorMessage = "Enter a valid Sri Lankan phone number (e.g. 0771234567 or +94771234567)!")]
    public string Phone { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Enter a valid email address!")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters!")]
    public string Email { get; init; } = string.Empty;
}
