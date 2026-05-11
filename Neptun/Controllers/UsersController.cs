using Microsoft.AspNetCore.Mvc;
using Neptun.Models;
using Neptun.Services;
using Neptun.DTOs;

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await userService.GetUserByIdAsync(userId);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto) 
    {
        var result = await userService.RegisterUserAsync(dto);
        if (result == null) return BadRequest("Email already in use.");
        return Ok(result);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(Guid userId, UserUpdateDto dto) 
    {
        var result = await userService.UpdateUserAsync(userId, dto);
        if (result == null) return NotFound("User not found or inactive.");
        return Ok(result);
    }

    [HttpPost("{userId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        var success = await userService.SetUserStatusAsync(userId, false);
        return success ? Ok(new { message = "User deactivated" }) : NotFound();
    }

    [HttpPost("{userId}/reactivate")]
    public async Task<IActionResult> Reactivate(Guid userId)
    {
        var success = await userService.SetUserStatusAsync(userId, true);
        return success ? Ok(new { message = "User reactivated" }) : NotFound();
    }
}