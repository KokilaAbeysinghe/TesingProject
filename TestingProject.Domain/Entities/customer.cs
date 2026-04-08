using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}