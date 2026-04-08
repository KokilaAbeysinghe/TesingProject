using System;
using System.Collections.Generic;
using System.Text;

using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface ISaleRepository
{
    Task<List<Sale>> GetAllSales();
    Task<Sale> GetSaleById(int id);
    Task CreateSale(Sale sale);
}