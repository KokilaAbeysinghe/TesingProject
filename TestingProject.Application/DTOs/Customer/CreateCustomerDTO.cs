using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public class CreateCustomerDTO
{
    [Required(ErrorMessage = "Customer name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required!")]
    [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters!")]
    public string Phone { get; set; } = string.Empty;
}