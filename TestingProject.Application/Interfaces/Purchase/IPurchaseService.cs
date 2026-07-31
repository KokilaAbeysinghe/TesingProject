using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IPurchaseService
{
    Task<List<PurchaseDTO>> GetAllPurchases();
    Task<PagedResultDTO<PurchaseDTO>> GetPurchasesPaged(int pageNumber, int pageSize);
    Task<PurchaseDTO> GetPurchaseById(int id);
    Task CreatePurchase(CreatePurchaseDTO purchaseDTO);
}
