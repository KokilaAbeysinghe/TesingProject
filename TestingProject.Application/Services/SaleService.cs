using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
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
        var sale = new Sale
        {
            CustomerId = saleDTO.CustomerId,
            SaleDate = DateTime.Now,
            SaleItems = saleDTO.SaleItems.Select(si => new SaleItem
            {
                ProductId = si.ProductId,
                Quantity = si.Quantity
            }).ToList()
        };
        await _saleRepository.CreateSale(sale);
    }

    public async Task<decimal> CalculateTotal(int saleId)
    {
        var sale = await _saleRepository.GetSaleById(saleId);
        decimal total = 0;
        foreach (var item in sale.SaleItems)
        {
            total += item.UnitPrice * item.Quantity;
        }
        return total;
    }
}