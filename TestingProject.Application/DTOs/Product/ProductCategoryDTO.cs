using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Application.DTOs;

public class ProductCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}