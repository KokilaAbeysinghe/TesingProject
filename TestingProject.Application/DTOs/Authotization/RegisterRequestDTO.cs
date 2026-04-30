using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs.Auth;

public class RegisterRequestDTO
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string ContactNumber { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public string? Role { get; set; }
}