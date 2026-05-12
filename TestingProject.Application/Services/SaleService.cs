using TestingProject.Domain.Entities;
using TestingProject.Application.Interfaces;
using TestingProject.Application.DTOs;

namespace TestingProject.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public SaleService(ISaleRepository saleRepository,IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<List<SaleDTO>> GetAllSales()
    {
        var sales = await _saleRepository.GetAllSales();
        return sales.Select(s => new SaleDTO
        {
            Id = s.Id,
            SaleDate = s.SaleDate,
            CustomerName = s.Customer.Name,
            TotalAmount = s.TotalAmount,
            SaleItems = s.SaleItems.Select(si => new SaleItemDTO
            {
                ProductId = si.ProductId,
                ProductName = si.Product.Name,
                Quantity = si.Quantity,
                UnitPrice = si.UnitPrice
            }).ToList()
        }).ToList();
    }

    public async Task<SaleDTO> GetSaleById(int id)
    {
        var sale = await _saleRepository.GetSaleById(id);
        if (sale == null) return null!;
        return new SaleDTO
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            CustomerName = sale.Customer.Name,
            TotalAmount = sale.TotalAmount,
            SaleItems = sale.SaleItems.Select(si => new SaleItemDTO
            {
                ProductId = si.ProductId,
                ProductName = si.Product.Name,
                Quantity = si.Quantity,
                UnitPrice = si.UnitPrice
            }).ToList()
        };
    }

    public async Task CreateSale(CreateSaleDTO saleDTO)
    {
        var saleItems = new List<SaleItem>();

        foreach (var item in saleDTO.SaleItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);

            if (product is null)
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");

            saleItems.Add(new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        var sale = new Sale
        {
            CustomerId = saleDTO.CustomerId,
            SaleDate = DateTime.UtcNow,
            SaleItems = saleItems,
            TotalAmount = saleItems.Sum(si => si.UnitPrice * si.Quantity)
        };

        await _saleRepository.CreateSale(sale);
    }

    public async Task<decimal> CalculateTotal(int saleId)
    {
        var sale = await _saleRepository.GetSaleById(saleId);

        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {saleId} not found.");

        return sale.SaleItems.Sum(item => item.UnitPrice * item.Quantity);
    }
}