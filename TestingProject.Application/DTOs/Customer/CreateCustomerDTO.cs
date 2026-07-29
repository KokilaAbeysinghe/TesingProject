using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public class CreateCustomerDTO
{
    [Required(ErrorMessage = "Customer name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; set; } = string.Empty;


    [Required(ErrorMessage = "Customer last name is required!")]
    [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters!")]
    public string LastName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Phone number is required!")]
    [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters!")]
    [RegularExpression(@"^(?:\+94|0)[0-9]{9}$", ErrorMessage = "Enter a valid Sri Lankan phone number (e.g. 0771234567 or +94771234567)!")]
    public string Phone { get; set; } = string.Empty;
}