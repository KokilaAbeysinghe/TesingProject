using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllSuppliers();
    Task<Supplier?> GetSupplierById(int id);
    Task AddSupplier(Supplier supplier);
    Task UpdateSupplier(Supplier supplier);
    Task DeleteSupplier(int id);
}
