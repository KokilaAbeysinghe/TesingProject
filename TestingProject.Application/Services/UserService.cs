using TestingProject.Application.Interfaces;
using TestingProject.Domain.Entities;
using TestingProject.Application.DTOs;


namespace TestingProject.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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