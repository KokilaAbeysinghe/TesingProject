using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface ISupplierService
{
    Task<List<SupplierDTO>> GetAllSuppliers();
    Task<PagedResultDTO<SupplierDTO>> GetSuppliersPaged(int pageNumber, int pageSize, string? search);
    Task<SupplierDTO> GetSupplierById(int id);
    Task AddSupplier(CreateSupplierDTO supplierDTO);
    Task UpdateSupplier(int id, CreateSupplierDTO supplierDTO);
    Task DeleteSupplier(int id);
}
