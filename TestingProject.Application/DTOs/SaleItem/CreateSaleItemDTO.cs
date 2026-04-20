using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Application.DTOs;

public class CreateSaleItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}