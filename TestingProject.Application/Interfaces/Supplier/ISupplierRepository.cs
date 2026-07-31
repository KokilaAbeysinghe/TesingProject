using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllSuppliers();
    Task<(List<Supplier> Items, int TotalCount)> GetSuppliersPaged(int pageNumber, int pageSize, string? search);
    Task<Supplier?> GetSupplierById(int id);
    Task AddSupplier(Supplier supplier);
    Task UpdateSupplier(Supplier supplier);
    Task DeleteSupplier(int id);
}
