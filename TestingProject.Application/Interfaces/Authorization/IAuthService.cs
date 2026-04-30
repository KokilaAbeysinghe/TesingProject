using TestingProject.Application.DTOs.Auth;
using TestingProject.Application.DTOs.Authorization;

namespace TestingProject.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResponseDTO> Register(RegisterRequestDTO request);
    Task<AuthResponseDTO> Login(LoginRequestDTO request);
}