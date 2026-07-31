using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface IPurchaseRepository
{
    Task<List<Purchase>> GetAllPurchases();
    Task<(List<Purchase> Items, int TotalCount)> GetPurchasesPaged(int pageNumber, int pageSize);
    Task<Purchase?> GetPurchaseById(int id);
    Task CreatePurchase(Purchase purchase);
}
