using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IUserService
{  
    Task<UserDTO> GetUserById(int id);
    Task AddUser(CreateUserDTO userDTO);
    Task UpdateUser(int id, CreateUserDTO userDTO);
    Task DeleteUser(int id);
}
