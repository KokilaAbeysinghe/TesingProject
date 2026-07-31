using TestingProject.Application.DTOs;

namespace TestingProject.Application.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsers();
    Task<PagedResultDTO<UserDTO>> GetUsersPaged(int pageNumber, int pageSize);
    Task<UserDTO> GetUserById(int id);
    Task AddUser(CreateUserDTO userDTO);
    Task UpdateUser(int id, UpdateUserDTO userDTO);
    Task DeleteUser(int id);
}
