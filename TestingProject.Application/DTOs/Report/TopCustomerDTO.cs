
namespace TestingProject.Application.DTOs.Report
{
    public class TopCustomerDTO
    {
        public int CustomerId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public int QuantityBuy { get; init; }
        public decimal CustomerAmount { get; init; }
    }
}
