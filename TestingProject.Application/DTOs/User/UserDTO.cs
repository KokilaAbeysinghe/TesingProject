using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Application.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required string ContactNumber { get; set; } 
}
