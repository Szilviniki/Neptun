using Microsoft.EntityFrameworkCore;
using Neptun.Data; 
using Neptun.Models;
using Neptun.DTOs;
using System;

namespace Neptun.Services;

public class UserService(ApplicationDbContext context)
{
    public async Task<UserModel?> GetUserByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<UserModel?> RegisterUserAsync(UserRegisterDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Email == dto.Email))
            return null;

        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Type = dto.Type,
            Mode = dto.Mode,
            IsActive = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<UserModel?> UpdateUserAsync(Guid userId, UserUpdateDto dto)
    {
        var user = await context.Users.FindAsync(userId);

        if (user == null || !user.IsActive) return null;

        user.Username = dto.Username;
        user.Email = dto.Email;

        if (!string.IsNullOrEmpty(dto.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);


        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> SetUserStatusAsync(Guid id, bool active)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return false;

        user.IsActive = active;
        await context.SaveChangesAsync();
        return true;
    }
}