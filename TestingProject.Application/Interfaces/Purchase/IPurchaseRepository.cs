using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface IPurchaseRepository
{
    Task<List<Purchase>> GetAllPurchases();
    Task<Purchase?> GetPurchaseById(int id);
    Task CreatePurchase(Purchase purchase);
}
