using TestingProject.Domain.Entities;

namespace TestingProject.Application.Interfaces;
public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<User> GetUserByEmail(string email);
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(int id);
}
