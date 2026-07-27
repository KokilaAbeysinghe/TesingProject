using TestingProject.Domain.Enums;

namespace TestingProject.Application.DTOs;

public record PaymentMethodSummaryDTO
{
    public PaymentMethod PaymentMethod { get; init; }
    public int SalesCount { get; init; }
    public decimal TotalAmount { get; init; }
}
