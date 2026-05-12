using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;
using TestingProject.Application.Interfaces.Password;
using TestingProject.Domain.Entities;


namespace TestingProject.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashingService _passwordHashingService;

        public UserService(IUserRepository userRepository,IPasswordHashingService passwordHashingService)
        {
            _userRepository = userRepository;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found!");

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
            };
        }

        public async Task AddUser(CreateUserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                ContactNumber = userDTO.ContactNumber,
                Role = string.IsNullOrWhiteSpace(userDTO.Role) ? "Cashier" : userDTO.Role,
                PasswordHash = _passwordHashingService.HashPassword(userDTO.Password)
            };
            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(int id, CreateUserDTO userDTO)
        {
            var user = new User
            {
                Id = id,
                Name = userDTO.Name,
                Email = userDTO.Email,
                ContactNumber = userDTO.ContactNumber,
            };
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
        }
    }
}