using TestingProject.Domain.Entities;
namespace TestingProject.Application.Interfaces;

public interface ISaleRepository
{
    Task<List<Sale>> GetAllSales();
    Task<(List<Sale> Items, int TotalCount)> GetSalesPaged(int pageNumber, int pageSize);
    Task<Sale?> GetSaleById(int id);
    Task CreateSale(Sale sale);
    Task UpdateSale(Sale sale);
    Task<List<Sale>> GetSalesBetweenDates(DateTime startDate, DateTime endDate);
    Task<List<Sale>> GetAllSalesForSummary();
}                       