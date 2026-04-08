using System;
using System.Collections.Generic;
using System.Text;

namespace TestingProject.Application.DTOs;

public class CreateSaleDTO
{
    public int CustomerId { get; set; }
    public List<CreateSaleItemDTO> SaleItems { get; set; } = new();
}
