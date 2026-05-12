namespace TestingProject.Application.Interfaces.Password;

public interface IPasswordHashingService
{
    string HashPassword(string password);
}
