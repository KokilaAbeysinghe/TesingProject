using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public record UpdateUserDTO
{
    [Required(ErrorMessage = "User name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required!")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "User ContactNumber is required!")]
    [StringLength(10, ErrorMessage = "ContactNumber cannot exceed 10 characters!")]
    public string ContactNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "Role is required!")]
    public string Role { get; init; } = "Cashier";

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
    public string? Password { get; init; }
}
