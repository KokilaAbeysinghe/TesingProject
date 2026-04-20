using Microsoft.AspNetCore.Mvc;
using TestingProject.Application.Interfaces;
using TestingProject.Application.DTOs;


namespace TestingProject.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserContollers : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserContollers(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserById(id);
        if (user == null)
            return NotFound("User not found!");
        return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDTO userDTO)
        {
           await _userService.AddUser(userDTO);
           return Ok("User added successfully!");
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, CreateUserDTO userDTO)
        {
            await _userService.UpdateUser(id, userDTO);
            return Ok("User updated successfully!");
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok("User deleted successfully!");
        }













    }
