using System.ComponentModel.DataAnnotations;
using TestingProject.Domain.Enums;

namespace TestingProject.Application.DTOs;

public record UpdateSaleDTO
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid customer is required.")]
    public int CustomerId { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
}
