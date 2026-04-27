using Microsoft.AspNetCore.Mvc;
using Neptun.Models;
using Neptun.Services;

namespace Neptun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserModel user)
    {
        var result = await _userService.RegisterUserAsync(user);
        if (result == null) return BadRequest("Email already in use.");
        return Ok(result);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update(Guid userId, UserModel user)
    {
        var result = await _userService.UpdateUserAsync(userId, user);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{userId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        var success = await _userService.SetUserStatusAsync(userId, false);
        return success ? Ok(new { message = "User deactivated" }) : NotFound();
    }

    [HttpPost("{userId}/reactivate")]
    public async Task<IActionResult> Reactivate(Guid userId)
    {
        var success = await _userService.SetUserStatusAsync(userId, true);
        return success ? Ok(new { message = "User reactivated" }) : NotFound();
    }
}