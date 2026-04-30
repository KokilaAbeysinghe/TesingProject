using Microsoft.AspNetCore.Identity;
using TestingProject.Application.DTOs.Auth;
using TestingProject.Application.DTOs.Authorization;
using TestingProject.Application.Interfaces;
using TestingProject.Application.Interfaces.Auth;
using TestingProject.Domain.Entities;

namespace TestingProject.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly JwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IUserRepository users, JwtTokenService jwt)
    {
        _users = users;
        _jwt = jwt;
    }

    public async Task<AuthResponseDTO> Register(RegisterRequestDTO request)
    {
        // 1. Check email not already taken
        var existing = await _users.GetUserByEmail(request.Email);
        if (existing != null)
            throw new InvalidOperationException("Email is already registered.");

        // 2. Build the user entity
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            ContactNumber = request.ContactNumber,
            Role = string.IsNullOrWhiteSpace(request.Role) ? "Cashier" : request.Role,
        };

        // 3. Hash the password and save
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        await _users.AddUser(user);

        // 4. Issue token
        var (token, expiresAtUtc) = _jwt.CreateAccessToken(user);
        return new AuthResponseDTO
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AuthResponseDTO> Login(LoginRequestDTO request)
    {
        // 1. Find user by email
        var user = await _users.GetUserByEmail(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // 2. Verify password hash
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // 3. Issue token
        var (token, expiresAtUtc) = _jwt.CreateAccessToken(user);
        return new AuthResponseDTO
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }
}