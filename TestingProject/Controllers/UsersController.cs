using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.DTOs;
using TestingProject.Application.Interfaces;

namespace TestingProject.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();

        return Ok(users);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetUsersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _userService.GetUsersPaged(pageNumber, pageSize);

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserById(id);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDTO userDTO)
    {
        await _userService.AddUser(userDTO);

        return Ok(new { message = "User added successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO userDTO)
    {
        await _userService.UpdateUser(id, userDTO);

        return Ok(new { message = "User updated successfully!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);

        return Ok(new { message = "User deleted successfully!" });
    }
}
