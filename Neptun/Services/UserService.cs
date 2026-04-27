using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Models;
using BCrypt.Net;

namespace Neptun.Services;

public class UserService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;


    public async Task<UserModel?> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<UserModel?> RegisterUserAsync(UserModel user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return null;

        user.Id = Guid.NewGuid();
        user.IsActive = true;

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;

    }
    public async Task<UserModel?> UpdateUserAsync(Guid userId, UserModel updatedData)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        user.Username = updatedData.Username;
        user.Email = updatedData.Email;

        if (!string.IsNullOrEmpty(updatedData.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(updatedData.Password);

        user.Mode = updatedData.Mode;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> SetUserStatusAsync(Guid userId, bool status)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.IsActive = status;
        await _context.SaveChangesAsync();
        return true;
    }
}