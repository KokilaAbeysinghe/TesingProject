using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface ISaleService
{
    Task<List<SaleDTO>> GetAllSales();
    Task<SaleDTO> GetSaleById(int id);
    Task CreateSale(CreateSaleDTO saleDTO);
    Task<decimal> CalculateTotal(int saleId);
}