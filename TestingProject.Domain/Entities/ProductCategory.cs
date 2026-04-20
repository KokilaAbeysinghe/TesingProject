using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Domain.Entities;

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // one category has many products
    public List<Product> Products { get; set; } = new();
}