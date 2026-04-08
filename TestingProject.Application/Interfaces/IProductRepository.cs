using System;
using System.Collections.Generic;
using System.Text;

using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();
    Task<Product> GetProductById(int id);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int id);
}