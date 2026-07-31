using TestingProject.Domain.Entities;
using TestingProject.Domain.Enums;
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
        return sales.Select(MapToDto).ToList();
    }

    public async Task<PagedResultDTO<SaleDTO>> GetSalesPaged(int pageNumber, int pageSize)
    {
        var (sales, totalCount) = await _saleRepository.GetSalesPaged(pageNumber, pageSize);

        return new PagedResultDTO<SaleDTO>
        {
            Items = sales.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<SaleDTO> GetSaleById(int id)
    {
        var sale = await _saleRepository.GetSaleById(id);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {id} not found.");

        return MapToDto(sale);
    }

    public async Task CreateSale(CreateSaleDTO saleDTO)
    {
        var saleItems = new List<SaleItem>();

        foreach (var item in saleDTO.SaleItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);

            if (product is null)
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{product.Name}'. Available: {product.Stock}, Requested: {item.Quantity}.");

            saleItems.Add(new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        var subtotalAmount = saleItems.Sum(si => si.UnitPrice * si.Quantity);
        var discountAmount = Math.Round(subtotalAmount * saleDTO.DiscountPercentage / 100m, 2);

        var sale = new Sale
        {
            CustomerId = saleDTO.CustomerId,
            SaleDate = DateTime.UtcNow,
            SaleItems = saleItems,
            DiscountPercentage = saleDTO.DiscountPercentage,
            TotalAmount = subtotalAmount - discountAmount,
            PaymentMethod = saleDTO.PaymentMethod
        };

        await _saleRepository.CreateSale(sale);

        foreach (var item in saleDTO.SaleItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);
            await _productRepository.UpdateStock(item.ProductId, product!.Stock - item.Quantity);
        }
    }

    public async Task UpdateSale(int id, UpdateSaleDTO saleDTO)
    {
        var sale = await _saleRepository.GetSaleById(id);

        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {id} not found.");

        if (sale.Status == SaleStatus.Voided)
            throw new InvalidOperationException("A voided sale cannot be edited.");

        var subtotalAmount = sale.SaleItems.Sum(si => si.UnitPrice * si.Quantity);
        var discountAmount = Math.Round(subtotalAmount * saleDTO.DiscountPercentage / 100m, 2);

        sale.CustomerId = saleDTO.CustomerId;
        sale.PaymentMethod = saleDTO.PaymentMethod;
        sale.DiscountPercentage = saleDTO.DiscountPercentage;
        sale.TotalAmount = subtotalAmount - discountAmount;

        await _saleRepository.UpdateSale(sale);
    }

    public async Task VoidSale(int id)
    {
        var sale = await _saleRepository.GetSaleById(id);

        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {id} not found.");

        if (sale.Status == SaleStatus.Voided)
            throw new InvalidOperationException("Sale is already voided.");

        foreach (var item in sale.SaleItems)
        {
            var product = await _productRepository.GetProductById(item.ProductId);

            if (product is not null)
                await _productRepository.UpdateStock(item.ProductId, product.Stock + item.Quantity);
        }

        sale.Status = SaleStatus.Voided;

        await _saleRepository.UpdateSale(sale);
    }

    public async Task<decimal> CalculateTotal(int saleId)
    {
        var sale = await _saleRepository.GetSaleById(saleId);

        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {saleId} not found.");

        var subtotalAmount = sale.SaleItems.Sum(item => item.UnitPrice * item.Quantity);
        var discountAmount = Math.Round(subtotalAmount * sale.DiscountPercentage / 100m, 2);

        return subtotalAmount - discountAmount;
    }

    private static SaleDTO MapToDto(Sale sale)
    {
        var subtotalAmount = sale.SaleItems.Sum(si => si.UnitPrice * si.Quantity);

        return new SaleDTO
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.Customer.Name,
            SubtotalAmount = subtotalAmount,
            DiscountPercentage = sale.DiscountPercentage,
            DiscountAmount = Math.Round(subtotalAmount * sale.DiscountPercentage / 100m, 2),
            TotalAmount = sale.TotalAmount,
            PaymentMethod = sale.PaymentMethod,
            Status = sale.Status,
            SaleItems = sale.SaleItems.Select(si => new SaleItemDTO
            {
                ProductId = si.ProductId,
                ProductName = si.Product.Name,
                Quantity = si.Quantity,
                UnitPrice = si.UnitPrice
            }).ToList()
        };
    }
}