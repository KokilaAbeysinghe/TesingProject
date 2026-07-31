using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface ISaleService
{
    Task<List<SaleDTO>> GetAllSales();
    Task<PagedResultDTO<SaleDTO>> GetSalesPaged(int pageNumber, int pageSize);
    Task<SaleDTO> GetSaleById(int id);
    Task CreateSale(CreateSaleDTO saleDTO);
    Task UpdateSale(int id, UpdateSaleDTO saleDTO);
    Task VoidSale(int id);
    Task<decimal> CalculateTotal(int saleId);
}