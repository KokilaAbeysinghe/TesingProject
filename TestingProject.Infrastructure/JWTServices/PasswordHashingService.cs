using Microsoft.AspNetCore.Identity;
using TestingProject.Application.Interfaces.Password;
using TestingProject.Domain.Entities;

namespace TestingProject.Infrastructure.Services;

public class PasswordHashingService : IPasswordHashingService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new User { ContactNumber = string.Empty }, password);
    }
}