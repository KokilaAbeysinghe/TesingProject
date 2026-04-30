namespace TestingProject.Application.DTOs.Auth;

public class AuthResponseDTO
{
    public required string AccessToken { get; set; }
    public required DateTime ExpiresAtUtc { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}