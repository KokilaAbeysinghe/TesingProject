using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Application.Interfaces.Password;
using TestingProject.Domain.Entities;

namespace TestingProject.Application.Services;

public class UserService : IUserService
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "Admin",
        "Manager",
        "Cashier"
    };

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;

    public UserService(IUserRepository userRepository, IPasswordHashingService passwordHashingService)
    {
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<List<UserDTO>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();

        return users.Select(MapToDto).ToList();
    }

    public async Task<PagedResultDTO<UserDTO>> GetUsersPaged(int pageNumber, int pageSize)
    {
        var (users, totalCount) = await _userRepository.GetUsersPaged(pageNumber, pageSize);

        return new PagedResultDTO<UserDTO>
        {
            Items = users.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {id} not found!");

        return MapToDto(user);
    }

    public async Task AddUser(CreateUserDTO userDTO)
    {
        var existing = await _userRepository.GetUserByEmail(userDTO.Email);

        if (existing is not null)
            throw new InvalidOperationException("Email is already registered.");

        var role = NormalizeRole(userDTO.Role);

        var user = new User
        {
            Name = userDTO.Name,
            Email = userDTO.Email,
            ContactNumber = userDTO.ContactNumber,
            Role = role,
            PasswordHash = _passwordHashingService.HashPassword(userDTO.Password)
        };

        await _userRepository.AddUser(user);
    }

    public async Task UpdateUser(int id, UpdateUserDTO userDTO)
    {
        var existing = await _userRepository.GetUserById(id);

        if (existing is null)
            throw new KeyNotFoundException($"User with ID {id} not found!");

        var emailOwner = await _userRepository.GetUserByEmail(userDTO.Email);

        if (emailOwner is not null && emailOwner.Id != id)
            throw new InvalidOperationException("Email is already registered.");

        existing.Name = userDTO.Name;
        existing.Email = userDTO.Email;
        existing.ContactNumber = userDTO.ContactNumber;
        existing.Role = NormalizeRole(userDTO.Role);

        if (!string.IsNullOrWhiteSpace(userDTO.Password))
        {
            existing.PasswordHash = _passwordHashingService.HashPassword(userDTO.Password);
        }

        await _userRepository.UpdateUser(existing);
    }

    public async Task DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);
    }

    private static string NormalizeRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return "Cashier";

        var matchedRole = AllowedRoles.FirstOrDefault(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));

        if (matchedRole is null)
            throw new InvalidOperationException("Role must be Admin, Manager, or Cashier.");

        return matchedRole;
    }

    private static UserDTO MapToDto(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            ContactNumber = user.ContactNumber,
            Role = user.Role
        };
    }
}
