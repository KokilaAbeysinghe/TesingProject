using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public PurchaseService(
        IPurchaseRepository purchaseRepository,
        IProductRepository productRepository,
        ISupplierRepository supplierRepository)
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<List<PurchaseDTO>> GetAllPurchases()
    {
        var purchases = await _purchaseRepository.GetAllPurchases();

        return purchases.Select(MapToDto).ToList();
    }

    public async Task<PurchaseDTO> GetPurchaseById(int id)
    {
        var purchase = await _purchaseRepository.GetPurchaseById(id);

        if (purchase is null)
            throw new KeyNotFoundException($"Purchase with ID {id} not found.");

        return MapToDto(purchase);
    }

    public async Task CreatePurchase(CreatePurchaseDTO purchaseDTO)
    {
        var supplier = await _supplierRepository.GetSupplierById(purchaseDTO.SupplierId);

        if (supplier is null)
            throw new KeyNotFoundException($"Supplier with ID {purchaseDTO.SupplierId} not found.");

        var purchaseItems = new List<PurchaseItem>();

        foreach (var item in purchaseDTO.PurchaseItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);

            if (product is null)
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");

            purchaseItems.Add(new PurchaseItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost
            });
        }

        var purchase = new Purchase
        {
            SupplierId = purchaseDTO.SupplierId,
            PurchaseDate = DateTime.UtcNow,
            PurchaseItems = purchaseItems,
            TotalAmount = purchaseItems.Sum(item => item.UnitCost * item.Quantity)
        };

        await _purchaseRepository.CreatePurchase(purchase);

        foreach (var item in purchaseDTO.PurchaseItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);
            await _productRepository.UpdateStock(item.ProductId, product!.Stock + item.Quantity);
        }
    }

    private static PurchaseDTO MapToDto(Purchase purchase)
    {
        return new PurchaseDTO
        {
            Id = purchase.Id,
            PurchaseDate = purchase.PurchaseDate,
            SupplierId = purchase.SupplierId,
            SupplierName = purchase.Supplier.Name,
            TotalAmount = purchase.TotalAmount,
            PurchaseItems = purchase.PurchaseItems.Select(item => new PurchaseItemDTO
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost
            }).ToList()
        };
    }
}
