using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestingProject.Application.DTOs;

public class CreateUserDTO
{

    [Required(ErrorMessage = "User name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required!")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "User ContactNumber is required!")]
    [StringLength(10, ErrorMessage = "ContactNumber cannot exceed 10 characters!")]
    public string ContactNumber { get; set; } = string.Empty;
}
