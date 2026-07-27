using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<List<SupplierDTO>> GetAllSuppliers()
    {
        var suppliers = await _supplierRepository.GetAllSuppliers();

        return suppliers.Select(MapToDto).ToList();
    }

    public async Task<SupplierDTO> GetSupplierById(int id)
    {
        var supplier = await _supplierRepository.GetSupplierById(id);

        if (supplier is null)
            throw new KeyNotFoundException($"Supplier with ID {id} not found!");

        return MapToDto(supplier);
    }

    public async Task AddSupplier(CreateSupplierDTO supplierDTO)
    {
        var supplier = new Supplier
        {
            Name = supplierDTO.Name,
            Phone = supplierDTO.Phone,
            Email = supplierDTO.Email
        };

        await _supplierRepository.AddSupplier(supplier);
    }

    public async Task UpdateSupplier(int id, CreateSupplierDTO supplierDTO)
    {
        var supplier = new Supplier
        {
            Id = id,
            Name = supplierDTO.Name,
            Phone = supplierDTO.Phone,
            Email = supplierDTO.Email
        };

        await _supplierRepository.UpdateSupplier(supplier);
    }

    public async Task DeleteSupplier(int id)
    {
        await _supplierRepository.DeleteSupplier(id);
    }

    private static SupplierDTO MapToDto(Supplier supplier)
    {
        return new SupplierDTO
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone,
            Email = supplier.Email
        };
    }
}
