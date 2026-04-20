using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace TestingProject.Application.DTOs;

public class CreateProductCategoryDTO
{
    [Required(ErrorMessage = "Category name is required!")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters!")]
    public string Description { get; set; } = string.Empty;
}